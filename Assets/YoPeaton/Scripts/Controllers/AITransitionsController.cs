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

    public AIController SetController {
        set {
            aiEntity = value;
        }
    }

    public void CheckTransitions() {
        if (ShouldStop()) {
            if (aiEntity.GetCurrentCorosingZone && CanCrossCurrentCrossingZone()) {
                aiEntity.SwitchToState(AIState.Moving);
            }
        }
        else {
            if (!IsCrossingACrossWalk() && !CanCrossCurrentCrossingZone()) {
                aiEntity.SwitchToState(AIState.SlowDown);
            }
            else if (IsThereAObstacleUpFront()) {
                aiEntity.SwitchToState(AIState.SlowDown);
            }
            else {
                aiEntity.SwitchToState(AIState.Moving);
            }
        }
        // Check all the posible conditions for a transition in the state machine
    }

    private bool CanCrossCurrentCrossingZone() {
        bool canCross = true;
        if (aiEntity.GetCurrentCorosingZone) {
            canCross = aiEntity.GetCurrentCorosingZone.CanCross(aiEntity);
        }
        return canCross;
    }

    private bool ShouldStop() {
        bool stop = false;
        if (aiEntity.GetCurrentState.Equals(AIState.StopAtCrossWalk)) {
            if (aiEntity.GetCurrentCorosingZone && !CanCrossCurrentCrossingZone()) {
                stop = true;
            }
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

    private bool IsCrossingACrossWalk() {
        bool isCrossing = false;
        if (aiEntity.GetCurrentState.Equals(AIState.CrossingCrossWalk)) {
            isCrossing = true;
        }
        return isCrossing;
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
        return stop;
    }

    public void OnCrossWalkEntered() {
        if (aiEntity.GetCurrentState.Equals(AIState.CrossingCrossWalk)) {
            float randomNumber = Random.Range(0.0f, 1.0f) * 100;
            if (randomNumber >= 100 - stopProbability && !CanCrossCurrentCrossingZone()) {
                aiEntity.SwitchToState(AIState.StopAtCrossWalk);
            }
            else {
                aiEntity.SwitchToState(AIState.CrossingCrossWalk);
                aiEntity.GetCurrentCorosingZone.OnStartedCrossing(aiEntity);
            }
        }
    }
}
