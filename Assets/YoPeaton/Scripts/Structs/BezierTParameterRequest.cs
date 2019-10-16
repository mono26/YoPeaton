using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BezierTParameterRequest
{
    public Vector3 pointToCheck;
    public float startingT, endT, thresholdT;

    public BezierTParameterRequest(Vector3 _pointToCheck, float _startingT = 0.0f, float _endT = 1.0f, float _thresholdT = 0.0001f)
    {
        pointToCheck = _pointToCheck;
        startingT = _startingT;
        endT = _endT;
        thresholdT = _thresholdT;
    }
}
