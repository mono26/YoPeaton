using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PathConnectionPair
{
    [SerializeField]
    public BezierSpline path1;
    [SerializeField]
    public BezierSpline path2;

    public bool HasAMatchingPath(BezierSpline _pathToCheck) {
        return (_pathToCheck.Equals(path1) || _pathToCheck.Equals(path2));
    }

    public BezierSpline GetConnection(BezierSpline _pathToCheck) {
        return (_pathToCheck.Equals(path1)) ? path2 : path1;
    }
}
