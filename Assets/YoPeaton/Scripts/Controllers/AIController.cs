using UnityEngine;

public class AIController : EntityController
{
    [SerializeField]
    private AIStateMachine stateMachine = null;
    [SerializeField]
    private float maxDistanceToCheckForStop = 3.0f;
    [SerializeField]
    private LayerMask layersToCheckCollision;
    [SerializeField]
    private Crosswalk currentCrossingZone;
    [SerializeField]
    private float stopProbability = 50.0f;

    private void Update() {
        if (!ShouldStop()) {
            if (gameObject.CompareTag("Pedestrian")) {
                if (!CanCrossCurrentCrossingZone()) {
                    stateMachine.SwitchToState(AIState.SlowDown);
                }
                else if (IsThereAObstacleUpFront()) {
                    stateMachine.SwitchToState(AIState.SlowDown);
                }
                else {
                    stateMachine.SwitchToState(AIState.Moving);
                }
            }
            else {
                if (IsThereAObstacleUpFront()) {
                    stateMachine.SwitchToState(AIState.SlowDown);
                }
                else {
                    stateMachine.SwitchToState(AIState.Moving);
                }
            }
        }
        // Check all the posible conditions for a transition in the state machine
    }

    protected override bool ShouldStop() {
        bool stop = false;
        if (stateMachine.GetCurrentState.Equals(AIState.StopAtCrossWalk)) {
            if (currentCrossingZone && !CanCrossCurrentCrossingZone()) {
                stop = true;
            }
            else {
                stateMachine.SwitchToState(AIState.Moving);
            }
        }
        return stop;
    }

    protected override bool ShouldSlowDown() {
        bool slowdown = false;
        if (stateMachine.GetCurrentState.Equals(AIState.SlowDown)) {
            slowdown = true;
        }
        return slowdown;
    }

    private bool IsThereAObstacleUpFront() {
        bool stop = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, maxDistanceToCheckForStop, layersToCheckCollision);
        // Only for debuging, should be removed later.
        Color debugColor = Color.green;
        if (hits.Length > 0) {
            GameObject objectHit;
            for (int i = 0; i < hits.Length; i++) {
                objectHit = hits[i].collider.gameObject;
                if (objectHit && !objectHit.Equals(gameObject)) {
                    if (objectHit.CompareTag("Pedestrian") || objectHit.CompareTag("Car")) {
                        stop = true;
                        debugColor = Color.red;
                    }
                }
            }
        }
        // Only for debuging, should be removed later.
        DebugController.DrawDebugRay(transform.position, transform.right, maxDistanceToCheckForStop, debugColor);
        return stop;
    }

    private bool CanCrossCurrentCrossingZone() {
        bool canCross = true;
        if (currentCrossingZone) {
            canCross = currentCrossingZone.CanCross(this);
        }
        return canCross;
    }

    protected override void OnTriggerEnter2D(Collider2D _other) {
        base.OnTriggerEnter2D(_other);
        if (_other.CompareTag("HotZone")) {
            Crosswalk crossWalk = _other.transform.parent.parent.GetComponent<Crosswalk>();
            if (!crossWalk.JustFinishedCrossing(this)) {
                currentCrossingZone = crossWalk;
                if (gameObject.tag.Equals("Car")) {
                    float randomNumber = Random.Range(0.0f, 1.0f) * 100;
                    if (randomNumber > stopProbability && !CanCrossCurrentCrossingZone()) {
                        stateMachine.SwitchToState(AIState.StopAtCrossWalk);
                    }
                }
            }
        }
        if (gameObject.tag.Equals("Pedestrian") && _other.CompareTag("CrossWalk")) {
            Crosswalk crossWalk = _other.transform.parent.GetComponent<Crosswalk>();
            currentCrossingZone = crossWalk;
            if (!crossWalk.CanCross(this)) {
                stateMachine.SwitchToState(AIState.SlowDown);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _other) {
        if (_other.CompareTag("CrossWalk")) {
            if (currentCrossingZone) {
                Crosswalk crossWalk = _other.transform.parent.GetComponent<Crosswalk>();
                if (crossWalk && currentCrossingZone.Equals(crossWalk)) {
                    currentCrossingZone = null;
                }
            }
        }
    }
}
