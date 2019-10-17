using UnityEngine;

public class AIController : EntityController
{
    [SerializeField]
    private AIStateMachine stateMachine = null;
    [SerializeField]
    private float maxDistanceToCheckForStop = 3.0f;
    [SerializeField]
    private LayerMask layersToCheckCollision;

    private Crosswalk currentCrossingZone;

    private void Update() {
        if (IsThereAObstacleUpFront() || IsThereAPedestrianInTheCurrentCrossingZone()) {
            stateMachine.SwitchToState(AIState.SlowDown);
        }
        else {
            stateMachine.SwitchToState(AIState.Moving);
        }
        // Check all the posible conditions for a transition in the state machine
    }

    protected override bool ShouldStop() {
        return stateMachine.GetCurrentState.Equals(AIState.SlowDown);
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

    private bool IsThereAPedestrianInTheCurrentCrossingZone() {
        bool pedestrian = false;
        if (currentCrossingZone) {
            pedestrian = currentCrossingZone.CanCross(gameObject.tag);
        }
        return pedestrian;
    }

    protected override void OnTriggerEnter2D(Collider2D _other) {
        base.OnTriggerEnter2D(_other);
        if (_other.CompareTag("HotZone")) {
            currentCrossingZone = _other.GetComponent<Crosswalk>();
        }
    }
}
