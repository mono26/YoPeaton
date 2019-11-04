using UnityEngine;

public class AITransitionsController : MonoBehaviour
{
    [SerializeField]
    private float stopProbability = 100.0f;

#region Dependencies
    [SerializeField]
    private AIController aiEntity;
#endregion

    public AIController SetController {
        set {
            aiEntity = value;
        }
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
            if (!aiEntity.IsCrossingACrossWalk()) {
                if (aiEntity.IsThereAObstacleUpFront()) {
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
