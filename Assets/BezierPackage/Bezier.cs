using UnityEngine;

public static class Bezier {

	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * p0 +
			2f * oneMinusT * t * p1 +
			t * t * p2;
	}

	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		return
			2f * (1f - t) * (p1 - p0) +
			2f * t * (p2 - p1);
	}

	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float OneMinusT = 1f - t;
		return
			OneMinusT * OneMinusT * OneMinusT * p0 +
			3f * OneMinusT * OneMinusT * t * p1 +
			3f * OneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			3f * oneMinusT * oneMinusT * (p1 - p0) +
			6f * oneMinusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}

	/// <summary>
	/// Gets the lenght of a curve made of 4 points.
	/// </summary>
	/// <param name="p0">Point 1.</param>
	/// <param name="p1">Point 2.</param>
	/// <param name="p2">Point 3.</param>
	/// <param name="p3">Point 4.</param>
	/// <returns></returns>
	public static float GetTotalLenght(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		int steps = 50;
		float arclenght = 0.0f;
		float tIncrement = 1.0f / (float)steps;
		for (int i = 1; i <= steps; i++)
		{
			float t = (float)i / (float)steps;
			Vector3 point = GetPoint(p0, p1, p2, p3, t);
			Vector3 previousPoint = GetPoint(p0, p1, p2, p3, t - tIncrement);
			Debug.DrawLine(previousPoint, point, Color.magenta, 10.0f);
			arclenght += (point - previousPoint).magnitude;
		}
		return arclenght;
	}

	public static MinDistanceAtTPair GetClosestTParameter(BezierTParameterRequest request, MinDistanceAtTPair _minDistanceAtT) {
		float mid = (request.startingT + request.endT)/2.0f;
		// Base case for recursion.
		if ((request.endT - request.startingT) < request.thresholdT) {
			_minDistanceAtT.tParameter = mid;
			return _minDistanceAtT;
		}
		// The two halves have param range [start, mid] and [mid, end]. We decide which one to use by using a midpoint param calculation for each section.
		float paramA = (request.startingT+mid) / 2.0f;
		float paramB = (mid+request.endT) / 2.0f;
		Vector3 posA = GetPoint(request.p0, request.p1, request.p2, request.p3, paramA);
		Vector3 posB = GetPoint(request.p0, request.p1, request.p2, request.p3, paramB);
		float distASq = (posA - request.pointToCheck).sqrMagnitude;
		float distBSq = (posB - request.pointToCheck).sqrMagnitude;
		if (distASq < distBSq) {
			request.endT = mid;
			_minDistanceAtT.minSqrDistance = distASq;
		}		
		else {
			request.startingT = mid;
			_minDistanceAtT.minSqrDistance = distBSq;
		}		
		// The (tail) recursive call.
		return GetClosestTParameter(request, _minDistanceAtT);
	}
}