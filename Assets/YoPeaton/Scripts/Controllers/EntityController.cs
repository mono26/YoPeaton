﻿using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField]
    private IMovable movableComponent;
    [SerializeField]
    private FollowPath followComponent;
    [SerializeField]
    private float probabilityOfChanginDirection = 50.0f;

    float distanceTravelled = 0.0f;
    private bool move = true;

    protected IMovable GetMovableComponent {
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

    protected FollowPath GetFollowPathComponent {
        get {
            if (followComponent == null) {
                followComponent = GetComponent<FollowPath>();
                if (followComponent == null) {
                    DebugController.LogMessage(string.Format("Gameobject {0} doesn't have FollowPath component", gameObject.name));
                }
            }
            return followComponent;
        }
    }

    private void Start() {
        GetInitialValuesToStartPath();
    }

    private void GetInitialValuesToStartPath() {
        float timeOnCurrentPath = followComponent.GetTParameter(transform.position);
        Debug.Log("timeOnCurrentPath: " + timeOnCurrentPath);
        distanceTravelled = followComponent.GetLengthAt(timeOnCurrentPath);
        Debug.Log("distanceTravelled: " + distanceTravelled);
    }

    private void FixedUpdate() {
        if (ShouldStop()) {
            GetMovableComponent?.SlowDown();
        }
        else {
            GetMovableComponent?.SpeedUp();
        }
        if (GetFollowPathComponent) {
            distanceTravelled += GetMovableComponent.GetCurrentSpeed * Time.fixedDeltaTime;
            if (move) {
                float t = distanceTravelled / GetFollowPathComponent.GetPathLeght;
                GetMovableComponent?.MoveToPosition(GetFollowPathComponent.GetPosition(t));
            }
        }
    }
    private void OnPathChanged()
    {
        if (GetFollowPathComponent.HasPath()) {
            GetInitialValuesToStartPath();
        }
        else {
            move = false;
        }
    }

    protected abstract bool ShouldStop();

    protected virtual void OnTriggerEnter2D(Collider2D _other) {
        if (_other.CompareTag("ChangeOfDirection")) {
            if (Random.Range(0.0f, 1.0f) * 100 > probabilityOfChanginDirection) {
                DirectionChange directionChanger = _other.GetComponent<DirectionChange>();
                DirectionPathPair[] conections = directionChanger.Getconections;
                BezierSpline nextPath = conections[Random.Range(0, conections.Length)].path;
            }
        }
    }
}