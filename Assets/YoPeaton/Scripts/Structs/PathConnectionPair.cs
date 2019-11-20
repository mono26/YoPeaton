using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PathConnectionPair
{
    [SerializeField]
    public BezierSpline from;
    [SerializeField]
    public BezierSpline to;

    public bool IsConnectionForPath(BezierSpline _pathToCheck) {
        return (_pathToCheck.Equals(from));
    }

    public BezierSpline GetConnection(BezierSpline _pathToCheck) {
        return to;
    }
}
