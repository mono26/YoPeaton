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
                //aiEntity.CheckIfIsBreakingTheLaw();
            }
        }
        else {
            if (!IsCrossingACrossWalk() && IsThereAObstacleUpFront()) {
                aiEntity.SwitchToState(AIState.SlowDown);
            }
            else  {
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
                Vector3 offsetPosition = transform.position + (transform.right * (float)(((colliderRadius) * transform.localScale.x) + 0.1));
                //This is the distance the center of the circle is going to move.
                float distance = maxDistanceToCheckForStop - ((colliderRadius * 2) * transform.localScale.x);
                float checkRadius = (colliderRadius + colliderRadius/2) * transform.localScale.x;
                Vector3 startPosition = offsetPosition;
                RaycastHit2D castHit1 = new RaycastHit2D();
                RaycastHit2D castHit2 = new RaycastHit2D();
                for (int i = 0; i < 3; i++) {
                    // if (i.Equals(0)) {
                    //     castHit1 = Physics2D.Raycast((Vector2)startPosition, transform.right, distance, layersToCheckCollision);
                    // }
                    // else {
                    //     castHit1 = Physics2D.Raycast(startPosition + (transform.up * checkRadius/i), transform.right, distance, layersToCheckCollision);
                    //     castHit2 = Physics2D.Raycast(startPosition + (transform.up * -checkRadius/i), transform.right, distance, layersToCheckCollision);
                    // }
                    castHit1 = Physics2D.Raycast(startPosition + (transform.up * checkRadius/i), transform.right, distance, layersToCheckCollision);
                    DebugController.DrawDebugRay(startPosition + (transform.up * checkRadius/i), transform.right, distance, Color.magenta);
                    castHit2 = Physics2D.Raycast(startPosition + (transform.up * -checkRadius/i), transform.right, distance, layersToCheckCollision);
                    DebugController.DrawDebugRay(startPosition + (transform.up * -checkRadius/i), transform.right, distance, Color.magenta);
                    if (castHit1.collider || castHit2.collider) {
                        GameObject objectHit = (castHit1.collider) ? castHit1.collider.gameObject : castHit2.collider.gameObject;
                        Vector3 hitPoint = (castHit1.collider) ? castHit1.point : castHit2.point;
                        if (objectHit && !objectHit.Equals(gameObject)) {
                            if (objectHit.CompareTag("Pedestrian") || objectHit.CompareTag("Car")) {
                                if (objectHit.GetComponent<EntityController>().IsOnTheStreet) {
                                    DebugController.DrawDebugLine(startPosition, hitPoint, Color.magenta);
                                }
                            }
                        }
                    }
                }
                // castHit = Physics2D.CircleCast((Vector2)startPosition, checkRadius, transform.right, distance, layersToCheckCollision);
                // if (castHit) {
                //     GameObject objectHit = castHit.collider.gameObject;
                //     if (objectHit && !objectHit.Equals(gameObject)) {
                //         if (objectHit.CompareTag("Pedestrian") || objectHit.CompareTag("Car")) {
                //             if (objectHit.GetComponent<EntityController>().IsOnTheStreet) {
                //                 DebugController.DrawDebugLine(startPosition, castHit.point, Color.magenta);
                //                 stop = true;
                //                 DebugController.DrawDebugCircle(startPosition, checkRadius, Color.green);
                //                 DebugController.DrawDebugRay(startPosition, transform.right, distance, Color.green);
                //                 DebugController.DrawDebugCircle(startPosition + transform.right * distance, checkRadius, Color.green);
                //             }
                //         }
                //     }
                // }
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
