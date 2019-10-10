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

    public Vector3 GetNextDirection(float time) {
        if (time >= 1.0f)
        {
            OnPathCompleted();
        }
        return pathToFollow.GetDirection(time);
    }

    public Vector3 GetNextPosition(float time) {
        Vector3 point = pathToFollow.GetPoint(time);
        if (time >= 1.0f)
        {
            OnPathCompleted();
        }
        return point;
    }

    public BezierSpline GetNextPosiblePath() {
        DirectionPathPair[] connections = pathToFollow.Getconections;
        BezierSpline nextPath = connections[Random.Range(0, connections.Length)].path;
        if (nextPath) {
            pathToFollow = nextPath;
        }
        return nextPath;
    }

    private void OnPathCompleted() {
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
}
