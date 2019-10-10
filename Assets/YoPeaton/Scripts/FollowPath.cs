using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public delegate void FollowPathEvent();
    private FollowPathEvent PathCompletedEvent;

    [SerializeField]
    private BezierSpline pathToFollow = null;

    private bool moveComponent = true;
    private float pathLength;
    private float progressInPath = 0.0f;

    public float GetPathLeght {
        get {
            return pathLength;
        }
    }

    public float GetProgressInPath {
        get {
            return progressInPath;
        }
    }

    void Start() {
        pathLength = GetLength();
        progressInPath = 0.0f;
    }

    public Vector3 GetNextDirection(float time) {
        if (time >= 1.0f)
        {
            OnPathCompleted();
        }
        return pathToFollow.GetDirection(time);
    }

    public Vector3 GetNextPosition(float time) {
        Vector3 point = pathToFollow.GetPoint(time);
        progressInPath = time;
        if (time >= 1.0f)
        {
            OnPathCompleted();
        }
        return point;
    }

    public BezierSpline GetNextPosiblePath() {
        BezierSpline nextPath = null;
        DirectionPathPair[] connections = pathToFollow.Getconections;
        if (connections.Length > 0) {
            nextPath = connections[Random.Range(0, connections.Length)].path;
        }
        return nextPath;
    }

    private void OnPathCompleted() {
        BezierSpline nextPath = GetNextPosiblePath();
        if (nextPath) {
            pathLength = GetLength();
            progressInPath = 0.0f;
        }
        pathToFollow = nextPath;
        PathCompletedEvent?.Invoke();
    }

    public float GetLength() {
        return pathToFollow.GetLength();
    }

    /// <summary>
    /// Subscribe delegate to the OnPathComplete event.
    /// </summary>
    /// <param name="onPathCompleteAction"> Function delegate to subscribe.</param>
    public void OnPathCompleted(FollowPathEvent onPathCompleteAction) {
        PathCompletedEvent += onPathCompleteAction;
    }

    /// <summary>
    /// Unsubscribe delegate to the OnPathComplete event.
    /// </summary>
    /// <param name="onPathCompleteAction"> Function delegate to unsubscribe.</param>
    public void ClearFromPathCompleted(FollowPathEvent onPathCompleteAction) {
        PathCompletedEvent -= onPathCompleteAction;
    }

    public bool HasPath() {
        bool hasPath = false;
        if (pathToFollow) {
            hasPath = true;
        }
        return hasPath;
    }
}
