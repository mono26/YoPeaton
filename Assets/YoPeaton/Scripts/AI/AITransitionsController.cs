using System;
using UnityEngine;

public class AITransitionsController : MonoBehaviour
{
    public Action onStartedAskingForCross;

    [SerializeField]
    private float stopProbability = 100.0f;
    [SerializeField]
    private float askForCrossProbability = 0.0f;
    [SerializeField]
    private float giveCrossProbability = 0.0f;
    [SerializeField]
    private float waitForClearCrossProbability = 100.0f;

#region Dependencies
    [SerializeField]
    private AIController aiEntity;
#endregion

    private bool alreadyAskedForPass = false;
    private bool alreadyGaveCross = true;

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
        if (aiEntity.GetCurrentState.Equals(AIState.WaitingAtCrossWalkAndAskingForPass)) {
            // TODO: Run probability of wait for clear pass?!
            if (alreadyAskedForPass && aiEntity.GetCurrentCrossingZone) {
                if (!ShouldWaitForClearCross() || CanCrossCurrentCrossingZone()) {
                    aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
                    aiEntity.SwitchToState(AIState.CrossingCrossWalk);
                    alreadyAskedForPass = false;
                    //aiEntity.CheckIfIsBreakingTheLaw();
                }
            }
        }
        else if (aiEntity.GetCurrentState.Equals(AIState.WaitingAtCrossWalk)) {
            if (aiEntity.GetEntityType.Equals(EntityType.Pedestrian)) {
                if (!alreadyAskedForPass && ShouldAskForCross()) {
                    aiEntity.SwitchToState(AIState.WaitingAtCrossWalkAndAskingForPass);
                    onStartedAskingForCross?.Invoke();
                    alreadyAskedForPass = true;
                }
            }
            if (aiEntity.GetCurrentCrossingZone && CanCrossCurrentCrossingZone()) {
                // TODO: Run probability for letting an entity asking for pass cross?!
                EntityType oherEntity = (aiEntity.GetEntityType.Equals(EntityType.Car)) ? EntityType.Pedestrian : EntityType.Car;
                bool giveCross = ShouldGiveCross();
                bool someoneIsWaiting = aiEntity.GetCurrentCrossingZone.IsThereAEntityAskingForCross(oherEntity);
                if (alreadyGaveCross || !giveCross || !someoneIsWaiting) {
                    aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
                    aiEntity.SwitchToState(AIState.CrossingCrossWalk);
                    //aiEntity.CheckIfIsBreakingTheLaw();
                }
                else if (giveCross && !alreadyGaveCross) {
                    aiEntity.GetCurrentCrossingZone.OnEntityGivingCross(aiEntity);
                }
            }
        }
        else if (!aiEntity.IsCrossingACrossWalk()) {
            if (aiEntity.IsThereAObstacleUpFront()) {
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
        if (aiEntity.GetCurrentCrossingZone) {
            canCross = aiEntity.GetCurrentCrossingZone.CanCross(aiEntity);
        }
        return canCross;
    }

    public void OnCrossWalkEntered() {
        if (!aiEntity.GetCurrentState.Equals(AIState.CrossingCrossWalk)) {
            float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
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

    /// <summary>
    /// Use to run a probality to see if the ai should ask for cross in the crosswalk.
    /// </summary>
    /// <returns></returns>
    private bool ShouldAskForCross() {
        float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
        bool askForPass = false;
        if (!aiEntity.GetCurrentCrossingZone.GetWaitingTicket(aiEntity).gaveCross) {
            if (randomNumber >= 100 - askForCrossProbability) {
                askForPass = true;
            }
        }
        return askForPass;
    }

    /// <summary>
    /// Use to run a probality to see if the ai should allow another ai cross if it's asking for cross.
    /// </summary>
    /// <returns></returns>
    private bool ShouldGiveCross() {
        float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
        bool giveCross = false;
        if (aiEntity.GetCurrentCrossingZone.GetWaitingTicket(aiEntity).gaveCross) {
            giveCross = true;
        }
        else if (randomNumber >= 100 - giveCrossProbability) {
            giveCross = true;
        }
        return giveCross;
    }

    /// <summary>
    /// Use to run a probality to see if the ai should allow another ai cross if it's asking for cross.
    /// </summary>
    /// <returns></returns>
    private bool ShouldWaitForClearCross() {
        float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
        bool waitForClear = false;
        if (randomNumber >= 100 - waitForClearCrossProbability) {
            waitForClear = true;
        }
        return waitForClear;
    }
}
