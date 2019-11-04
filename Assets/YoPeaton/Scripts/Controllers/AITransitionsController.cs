using UnityEngine;

public class AITransitionsController : MonoBehaviour
{
    [SerializeField]
    private float stopProbability = 100.0f;
    [SerializeField]
    private float maxDistanceToCheckForStop = 3.0f;
    [SerializeField]
    private LayerMask layersToCheckCollision;

#region Dependencies
    [SerializeField]
    private AIController aiEntity;
#endregion

    private float colliderRadius;
    private RaycastHit2D[] obstacles;

    public AIController SetController {
        set {
            aiEntity = value;
        }
    }

    private void Awake() {
        colliderRadius = GetComponent<CircleCollider2D>().radius;
    }

    // private void Start() {
    //     if (gameObject.CompareTag("Car")) {
    //         layersToCheckCollision = (1 << LayerMask.NameToLayer("Car")) | (1 << LayerMask.NameToLayer("Pedestrian"));
    //     }
    //     else {
    //         layersToCheckCollision = (1 << LayerMask.NameToLayer("Car"));
    //     }
    // }

    public void CheckTransitions() {
        if (ShouldStop()) {
            if (aiEntity.GetCurrentCrossingZone && CanCrossCurrentCrossingZone()) {
                aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
                aiEntity.SwitchToState(AIState.CrossingCrossWalk);
                aiEntity.CheckIfIsBreakingTheLaw();
            }
        }
        else {
            if (!IsCrossingACrossWalk()) {
                if (IsThereAObstacleUpFront()) {
                    aiEntity.SwitchToState(AIState.SlowDown);
                }
                else {
                    aiEntity.SwitchToState(AIState.Moving);
                }
            }
        }
        // Check all the posible conditions for a transition in the state machine
    }

    private bool CanCrossCurrentCrossingZone() {
        bool canCross = true;
        if (aiEntity.GetCurrentCrossingZone) {
            canCross = aiEntity.GetCurrentCrossingZone.CanCross(aiEntity);
        }
        return canCross;
    }

    private bool ShouldStop() {
        bool stop = false;
        if (aiEntity.GetCurrentState.Equals(AIState.WaitingAtCrossWalk)) {
            stop = true;
        }
        return stop;
    }

    private bool ShouldSlowDown() {
        bool slowdown = false;
        if (aiEntity.GetCurrentState.Equals(AIState.SlowDown)) {
            slowdown = true;
        }
        return slowdown;
    }

    public bool IsCrossingACrossWalk() {
        bool isCrossing = false;
        if (aiEntity.GetCurrentState.Equals(AIState.CrossingCrossWalk)) {
            isCrossing = true;
        }
        return isCrossing;
    }

    /// <summary>
    /// Checks for a obstacle ahead.true If it's a pedestrian or car stops.
    /// </summary>
    /// <returns>True if there is a obstacle ahead. False if not.</returns>
    private bool IsThereAObstacleUpFront() {
        bool stop = false;
        if (aiEntity.IsOnTheStreet) {
            if (gameObject.CompareTag("Pedestrian") && IsCrossingACrossWalk()) {
                //
            }
            else {
                // Wee have to calculate new distance and starposition values because we want to avoid detecting our selfs.
                Vector3 startPosition = transform.position + (transform.right * (float)(((colliderRadius) * transform.localScale.x) + 0.1));
                float distance = maxDistanceToCheckForStop - ((colliderRadius * 2) * transform.localScale.x);
                float checkWidth = ((colliderRadius + colliderRadius/2) * 2) * transform.localScale.x;
                GameObject obstacle = PhysicsHelper.RayCastOverALineForFirstGameObject(gameObject, startPosition, transform.up, checkWidth, transform.right, distance, layersToCheckCollision, 5);
                if (obstacle) {
                    if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car")) {
                        if (obstacle.GetComponent<EntityController>().IsOnTheStreet) {
                            stop = true;
                        }
                    }
                }
            }
        }
        return stop;
    }

    public void OnCrossWalkEntered() {
        if (!aiEntity.GetCurrentState.Equals(AIState.CrossingCrossWalk)) {
            float randomNumber = Random.Range(0.0f, 1.0f) * 100;
            bool canCross = CanCrossCurrentCrossingZone();
            if (randomNumber >= 100 - stopProbability && !canCross) {
                aiEntity.SwitchToState(AIState.WaitingAtCrossWalk);
            }
            else {
                aiEntity.SwitchToState(AIState.CrossingCrossWalk);
                aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
                // aiEntity.CheckIfIsBreakingTheLaw();
            }
        }
    }
}
