using UnityEngine;

[ExecuteInEditMode]
public class Path : MonoBehaviour
{
    [SerializeField]
    public BezierSpline Spline { get; private set; }

    private void Awake()
    {
        Spline = GetComponent<BezierSpline>();
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
            Vector3 point = Spline.GetPoint(t);
            Vector3 previousPoint = Spline.GetPoint(t - tIncrement);
            splineLenght += (point - previousPoint).magnitude;
        }
        return splineLenght;
    }

    public float GetLengthAt(float _tParameter)
    {
        return _tParameter * GetLength();
    }

    public float GetTParameter(Vector3 _pointToCheck)
    {
        MinDistanceAtTPair result = new MinDistanceAtTPair(float.NaN, float.NaN);
        for (int i = 0; i < Spline.ControlPointCount - 1; i += 3)
        {
            BezierTParameterRequest request = new BezierTParameterRequest(_pointToCheck);
            result = GetClosestTParameter(request, result);
        }
        return result.tParameter;
    }

    private MinDistanceAtTPair GetClosestTParameter(BezierTParameterRequest _request, MinDistanceAtTPair _compareWith)
    {
        float mid = (_request.startingT + _request.endT) / 2.0f;
        // Base case for recursion.
        if ((_request.endT - _request.startingT) < _request.thresholdT)
        {
            _compareWith.tParameter = mid;
            return _compareWith;
        }
        // The two halves have param range [start, mid] and [mid, end]. We decide which one to use by using a midpoint param calculation for each section.
        float paramA = (_request.startingT + mid) / 2.0f;
        float paramB = (mid + _request.endT) / 2.0f;
        Vector3 posA = Spline.GetPoint(paramA);
        Vector3 posB = Spline.GetPoint(paramB);
        float distASq = (posA - _request.pointToCheck).sqrMagnitude;
        float distBSq = (posB - _request.pointToCheck).sqrMagnitude;
        if (distASq < distBSq)
        {
            _request.endT = mid;
            _compareWith.minSqrDistance = distASq;
        }
        else
        {
            _request.startingT = mid;
            _compareWith.minSqrDistance = distBSq;
        }
        // The (tail) recursive call.
        return GetClosestTParameter(_request, _compareWith);
    }

    public Vector3 GetDirectionAt(float _tParameter)
    {
        return Spline.GetDirection(_tParameter);
    }

    public Vector3 GetPointAt(float _tParameter)
    {
        return Spline.GetPoint(_tParameter);
    }
}
