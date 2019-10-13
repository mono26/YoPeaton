using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BezierTParameterRequest
{
    public Vector3 p0, p1, p2, p3, pointToCheck;
    public float startingT, endT, thresholdT;

    public BezierTParameterRequest(Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3, Vector3 _pointToCheck, float _startingT = 0.0f, float _endT = 1.0f, float _thresholdT = 0.0001f)
    {
        p0 = _p0;
        p1 = _p1;
        p2 = _p2;
        p3 = _p3;
        pointToCheck = _pointToCheck;
        startingT = _startingT;
        endT = _endT;
        thresholdT = _thresholdT;
    }
}
