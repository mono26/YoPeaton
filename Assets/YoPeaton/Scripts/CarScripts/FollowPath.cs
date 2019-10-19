using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    private Action OnPathChanged;

    [SerializeField]
    private BezierSpline pathToFollow = null;

    private bool moveComponent = true;
    private float pathLength;

    public float GetPathLeght {
        get {
            return pathLength;
        }
    }

    public BezierSpline SetPath {
        set {
            pathToFollow = value;
            pathLength = GetLength();
        }
    }

    void Start() {
        pathLength = GetLength();
    }

    public Vector3 GetDirection(float time) {
        return pathToFollow.GetDirection(time);
    }

    public Vector3 GetPosition(float time) {
        return pathToFollow.GetPoint(time);
    }

    private void OnDirectionChanged(BezierSpline _nextPath) {
        pathToFollow = _nextPath;
        if (_nextPath) {
            pathLength = GetLength();
        }
        else {
            pathLength = 0.0f;
        }
        OnPathChanged?.Invoke();
    }

    public float GetLength() {
        return pathToFollow.GetLength();
    }

    public bool HasPath() {
        bool hasPath = false;
        if (pathToFollow) {
            hasPath = true;
        }
        return hasPath;
    }

    public float GetTParameter(Vector3 _pointToCheck) {
        return pathToFollow.GetTParameter(_pointToCheck);
    }

    public float GetLengthAt(float _tParameter) {
        return pathToFollow.GetLengthAt(_tParameter);
    }
}
