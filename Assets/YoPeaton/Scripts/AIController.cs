using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private IMovable movableComponent;
    [SerializeField]
    private FollowPath followComponent;

    float distanceTravelled;
    private float timeOnCurrentPath;
    private bool move = true;

    public IMovable GetMovableComponent {
        get {
            if (movableComponent == null) {
                movableComponent = GetComponent<IMovable>();
                if (movableComponent == null) {
                    DebugController.LogMessage(string.Format("Gameobject {0} doesn't have IMovable component", gameObject.name));
                }
            }
            return movableComponent;
        }
    }

    private void OnEnable() {
        followComponent.OnPathCompleted(OnPathCompleted);
    }

    private void OnDisable() {
        followComponent.ClearFromPathCompleted(OnPathCompleted);
    }

    private void FixedUpdate() {
        if (ShouldStop()) {
            GetMovableComponent?.SlowDown();
        }
        else {
            GetMovableComponent?.SpeedUp();
        }
        if (followComponent) {
            // float timeToFinishPath = followComponent.GetPathLeght / GetMovableComponent.GetCurrentSpeed;
            distanceTravelled += GetMovableComponent.GetCurrentSpeed * Time.fixedDeltaTime;
            // if (float.IsNaN(timeToFinishPath)) {
            //     timeToFinishPath = 0.0f;
            // }
            if (move) {
                // float t = timeOnCurrentPath / timeToFinishPath;
                float t = distanceTravelled / followComponent.GetPathLeght;
                // timeOnCurrentPath += Time.fixedDeltaTime;
                GetMovableComponent?.MoveToPosition(followComponent.GetNextPosition(t));
            }
        }
    }

    private bool ShouldStop() {
        // Code to look if the controller needs to stop.
        return false;
    }

    private void OnPathCompleted()
    {
        if (followComponent.HasPath()) {
            timeOnCurrentPath = 0.0f;
            distanceTravelled = 0.0f;
        }
        else {
            move = false;
        }
    }
}
