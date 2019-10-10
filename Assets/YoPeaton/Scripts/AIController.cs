using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private IMovable movableComponent;
    [SerializeField]
    private FollowPath followComponent;

    private bool move = true;

    private void OnEnable() {
        followComponent.OnPathCompleted(OnPathCompleted);
    }

    private void OnDisable() {
        followComponent.ClearFromPathCompleted(OnPathCompleted);
    }

    private void FixedUpdate() {
        if (ShouldStop()) {
            movableComponent.SlowDown();
        }
        else {
            movableComponent.SpeedUp();
        }
        float timeToFinishPath = followComponent.GetPathLeght / movableComponent.GetCurrentSpeed;
        if (float.IsNaN(timeToFinishPath))
        {
            timeToFinishPath = 0.0f;
        }
        if (move)
        {
            float t = followComponent.GetProgressInPath / timeToFinishPath;
            // MoveWithDirection(followComponent.GetNextDirection(t));
            Vector3 nextPosition = Vector3.Lerp(transform.position, followComponent.GetNextPosition(t), t);
            Debug.DrawRay(nextPosition, Vector3.right, Color.red, 10.0f);
            Debug.DrawRay(nextPosition, Vector3.up, Color.red, 10.0f);
            Debug.Log(t);
            movableComponent.MoveToPosition(nextPosition);
        }
    }

    private bool ShouldStop() {
        // Code to look if the controller needs to stop.
        return false;
    }

    private void OnPathCompleted()
    {
        if (followComponent.HasPath()) {
            move = false;
        }
        else {

        }
    }
}
