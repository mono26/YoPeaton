using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour {
	[SerializeField]
	private Vector3[] points;
	[SerializeField]
	private BezierControlPointMode[] modes;

	[SerializeField]
	private bool loop;

	public bool Loop {
		get {
			return loop;
		}
		set {
			loop = value;
			if (value == true) {
				modes[modes.Length - 1] = modes[0];
				SetControlPoint(0, points[0]);
			}
		}
	}

	public int ControlPointCount {
		get {
			return points.Length;
		}
	}

	private void OnDrawGizmos() {
		int steps = 50;
		Vector3 previousPoint = transform.position;
		Vector3 point = transform.position;
		for (int i = 0; i < steps; i++) {
			point = GetPoint((float)i / (float)steps);
			Vector3 arrowDirection = (point - previousPoint).normalized;
			Quaternion arrowAngle1 = Quaternion.Euler(0, 0, 45.0f);
			Quaternion arrowAngle2 = Quaternion.Euler(0, 0, -45.0f);
			Vector3 side1 = arrowAngle1 * -arrowDirection;
			Vector3 side2 = arrowAngle2 * -arrowDirection;
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(previousPoint, point);
			Gizmos.DrawRay(point, side1 * 0.1f);
			Gizmos.DrawRay(point, side2 * 0.1f);
			previousPoint = point;
		}
		Matrix4x4 matrix = transform.localToWorldMatrix;
		for (int i = 0; i < points.Length; i++) {
			point = points[i];
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(matrix.MultiplyPoint(point), 0.15f); 
		}
	}

	public Vector3 GetControlPoint (int index) {
		return points[index];
	}

	public void SetControlPoint (int index, Vector3 point) {
		if (index % 3 == 0) {
			Vector3 delta = point - points[index];
			if (loop) {
				if (index == 0) {
					points[1] += delta;
					points[points.Length - 2] += delta;
					points[points.Length - 1] = point;
				}
				else if (index == points.Length - 1) {
					points[0] = point;
					points[1] += delta;
					points[index - 1] += delta;
				}
				else {
					points[index - 1] += delta;
					points[index + 1] += delta;
				}
			}
			else {
				if (index > 0) {
					points[index - 1] += delta;
				}
				if (index + 1 < points.Length) {
					points[index + 1] += delta;
				}
			}
		}
		points[index] = point;
		EnforceMode(index);
	}

	public BezierControlPointMode GetControlPointMode (int index) {
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode (int index, BezierControlPointMode mode) {
		int modeIndex = (index + 1) / 3;
		modes[modeIndex] = mode;
		if (loop) {
			if (modeIndex == 0) {
				modes[modes.Length - 1] = mode;
			}
			else if (modeIndex == modes.Length - 1) {
				modes[0] = mode;
			}
		}
		EnforceMode(index);
	}

	private void EnforceMode (int index) {
		int modeIndex = (index + 1) / 3;
		BezierControlPointMode mode = modes[modeIndex];
		if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
			return;
		}

		int middleIndex = modeIndex * 3;
		int fixedIndex, enforcedIndex;
		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			if (fixedIndex < 0) {
				fixedIndex = points.Length - 2;
			}
			enforcedIndex = middleIndex + 1;
			if (enforcedIndex >= points.Length) {
				enforcedIndex = 1;
			}
		}
		else {
			fixedIndex = middleIndex + 1;
			if (fixedIndex >= points.Length) {
				fixedIndex = 1;
			}
			enforcedIndex = middleIndex - 1;
			if (enforcedIndex < 0) {
				enforcedIndex = points.Length - 2;
			}
		}

		Vector3 middle = points[middleIndex];
		Vector3 enforcedTangent = middle - points[fixedIndex];
		if (mode == BezierControlPointMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
		}
		points[enforcedIndex] = middle + enforcedTangent;
	}

	public int CurveCount {
		get {
			return (points.Length - 1) / 3;
		}
	}

	public Vector3 GetPoint (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
	}
	
	public Vector3 GetVelocity (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
	}
	
	/// <summary>
	/// Gets the direction at a point t inside the bezier spline.
	/// </summary>
	/// <param name="t">t parameter of the Bezier curve. B(t), must be clamped to 0 and 1. Being 0 the start and 1 the end.</param>
	/// <returns></returns>
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

	public void AddCurve () {
		Vector3 point = points[points.Length - 1];
		Array.Resize(ref points, points.Length + 3);
		point.x += 1f;
		points[points.Length - 3] = point;
		point.x += 1f;
		points[points.Length - 2] = point;
		point.x += 1f;
		points[points.Length - 1] = point;

		Array.Resize(ref modes, modes.Length + 1);
		modes[modes.Length - 1] = modes[modes.Length - 2];
		EnforceMode(points.Length - 4);

		if (loop) {
			points[points.Length - 1] = points[0];
			modes[modes.Length - 1] = modes[0];
			EnforceMode(0);
		}
	}
	
	public void Reset () {
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
		modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
	}

	/// <summary>
	/// Returns the lenght of the whole spline.
	/// </summary>
	/// <returns></returns>
	public float GetLength()
	{
		int steps = 50;
		float splineLenght = 0.0f;
		float tIncrement = 1.0f / (float)steps;
		for (int i = 1; i <= steps; i++)
		{
			float t = (float)i / (float)steps;
			Vector3 point = GetPoint(t);
			Vector3 previousPoint = GetPoint(t - tIncrement);
			splineLenght += (point - previousPoint).magnitude;
		}
		return splineLenght;
	}

	public float GetLengthAt(float _tParameter) {
		return _tParameter * GetLength();
	}

	public float GetTParameter(Vector3 _pointToCheck) {
		MinDistanceAtTPair result = new MinDistanceAtTPair(float.NaN, float.NaN);
		for (int i = 0; i < ControlPointCount - 1; i += 3)
		{
			BezierTParameterRequest request = new BezierTParameterRequest(_pointToCheck);
			result = GetClosestTParameter(request, result);
		}
		return result.tParameter;
	}

	private MinDistanceAtTPair GetClosestTParameter(BezierTParameterRequest request, MinDistanceAtTPair _minDistanceAtT) {
		float mid = (request.startingT + request.endT)/2.0f;
		// Base case for recursion.
		if ((request.endT - request.startingT) < request.thresholdT) {
			_minDistanceAtT.tParameter = mid;
			return _minDistanceAtT;
		}
		// The two halves have param range [start, mid] and [mid, end]. We decide which one to use by using a midpoint param calculation for each section.
		float paramA = (request.startingT+mid) / 2.0f;
		float paramB = (mid+request.endT) / 2.0f;
		Vector3 posA = GetPoint(paramA);
		Vector3 posB = GetPoint(paramB);
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