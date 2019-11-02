using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransitionsController : MonoBehaviour
{
    [SerializeField]
    private float stopProbability = 100.0f;
    [SerializeField]
    private float maxDistanceToCheckForStop = 3.0f;
    [SerializeField]
    private int layersToCheckCollision;

#region Dependencies
    [SerializeField]
    private AIController aiEntity;
#endregion

    private float colliderRadius;
    private RaycastHit2D[] obstacles;
    private RaycastHit2D castHit;

    public AIController SetController {
        set {
            aiEntity = value;
        }
    }

    private void Awake() {
        colliderRadius = GetComponent<CircleCollider2D>().radius;
    }

    private void Start() {
        if (gameObject.CompareTag("Car")) {
            layersToCheckCollision = (1 << LayerMask.NameToLayer("Car")) | (1 << LayerMask.NameToLayer("Pedestrian"));
        }
        else {
            layersToCheckCollision = (1 << LayerMask.NameToLayer("Car"));
        }
    }

    public void CheckTransitions() {
        if (ShouldStop()) {
            if (aiEntity.GetCurrentCrossingZone && CanCrossCurrentCrossingZone()) {
                aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
                aiEntity.SwitchToState(AIState.CrossingCrossWalk);
                //aiEntity.CheckIfIsBreakingTheLaw();
            }
        }
        else {
            if (IsThereAObstacleUpFront()) {
                aiEntity.SwitchToState(AIState.SlowDown);
            }
            else if (!IsCrossingACrossWalk()) {
                aiEntity.SwitchToState(AIState.Moving);
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

    private bool IsThereAObstacleUpFront() {
        bool stop = false;
        if (aiEntity.IsOnTheStreet) {
            if (gameObject.CompareTag("Pedestrian") && IsCrossingACrossWalk()) {
                //
            }
            else {
                Vector3 startPosition = transform.position + (transform.right * (float)(((colliderRadius * 2) * transform.localScale.x) + 0.1));
                float distance = maxDistanceToCheckForStop - ((colliderRadius * 2) * transform.localScale.x);
                float checkRadius = (colliderRadius + colliderRadius/2) * transform.localScale.x;
                castHit = Physics2D.CircleCast((Vector2)startPosition, checkRadius, transform.right, distance, layersToCheckCollision);
                if (castHit) {
                    GameObject objectHit = castHit.collider.gameObject;
                    if (objectHit && !objectHit.Equals(gameObject)) {
                        if (objectHit.CompareTag("Pedestrian") || objectHit.CompareTag("Car")) {
                            if (objectHit.GetComponent<EntityController>().IsOnTheStreet) {
                                DebugController.DrawDebugLine(startPosition, castHit.point, Color.yellow);
                                stop = true;
                            }
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
