using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public Action onPathChanged;

    [SerializeField]
    private Path pathToFollow = null;
    [SerializeField]
    private LayerMask directionChangeLayer;

    private bool moveComponent = true;
    private float pathLength;

    public float GetPathLeght {
        get {
            return pathLength;
        }
    }

    public Path SetPath {
        set {
            pathToFollow = value;
            pathLength = GetLength();
        }
    }

    public Path GetPath {
        get {
            return pathToFollow;
        }
    }

    void Start() {
        pathLength = GetLength();
    }

    /// <summary>
    /// Gets the direction at a point t inside the bezier spline.
    /// </summary>
    /// <param name="t">t parameter of the Bezier curve. B(t), must be clamped to 0 and 1. Being 0 the start and 1 the end.</param>
    /// <returns></returns>
    public Vector3 GetDirection(float time) 
    {
        Vector3 directionToReturn = Vector3.zero;
        if (pathToFollow) 
        {
            directionToReturn = pathToFollow.GetDirectionAt(time);
        }
        else
        {
            DebugController.LogErrorMessage("There is no path to follow reference");
        }
        return directionToReturn;
    }

    /// <summary>
    /// Gets the position at t inside the bezier spline.
    /// </summary>
    /// <param name="t">t parameter of the Bezier curve. B(t), must be clamped to 0 and 1. Being 0 the start and 1 the end.</param>
    /// <returns></returns>
    public Vector3 GetPosition(float _time) {
        return pathToFollow.GetPointAt(_time);
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

    public bool IsTheEndOfPath(Vector3 _pointToCheck) {
        return !(pathToFollow.GetTParameter(_pointToCheck) < 0.95f);
    }

    public bool IsThereOtherChangeOfDirection()
    {
        Vector3 endOfPath = pathToFollow.GetPointAt(1.0f);
        Vector3 directionVector = endOfPath - transform.position;
        GameObject directionChange = PhysicsHelper.RayCastForFirstGameObject(gameObject, transform.position, directionVector.normalized, directionVector.magnitude, directionChangeLayer);
        return directionChange != null;
    }
}
