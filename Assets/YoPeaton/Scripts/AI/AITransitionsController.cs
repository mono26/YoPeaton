using System;
using System.Collections;
using UnityEngine;

public class AITransitionsController : MonoBehaviour
{
    public Action<Vector3> onStartedAskingForCross;

    [SerializeField]
    private float askForCrossProbability = 0.0f;
    [SerializeField]
    private float askedForCrossWaitTime = 3.0f;
    [SerializeField]
    private float avoidCollisionProbability = 100.0f;
    [SerializeField]
    private float giveCrossProbability = 0.0f;
    [SerializeField]
    private float waitForClearCrossProbability = 100.0f;

    #region Dependencies
    [SerializeField]
    private AIController aiEntity;
    #endregion

    private bool alreadyGaveCross = false;
    private bool canCrossAfterWait = true;
    private WaitForSeconds askedForCrossWait;

    public AIController SetController
    {
        set
        {
            aiEntity = value;
        }
    }

    private void Start()
    {
        askedForCrossWait = new WaitForSeconds(askedForCrossWaitTime);
    }

    public void CheckTransitions()
    {
        // Waiting to cross and asking for cross.
        AIState currentState = aiEntity.GetCurrentState;
        ProcessState(currentState);
    }

    private void ProcessState(AIState _state)
    {
        // TODO refactor each state to a function.
        switch (_state)
        {
            case AIState.Crossing:
                {
                    if (ShouldAvoidCollision())
                    {
                        RaycastCheckResult result = IsThereAObstacle();
                        if (result.collided && result.otherEntity.IsCrossingCrosswalk)
                        {
                            aiEntity.SetMovementState(MovementState.SlowDown);
                        }
                        else
                        {
                            aiEntity.SetMovementState(MovementState.SpeedUp);
                        }
                        // Speed up
                    }
                    else
                    {
                        aiEntity.SetMovementState(MovementState.SpeedUp);
                    }
                    break;
                }
            case AIState.Moving:
                {
                    if (ShouldAvoidCollision())
                    {
                        RaycastCheckResult result = IsThereAObstacle();
                        if (result.collided && result.otherEntity.IsOnTheStreet)
                        {
                            aiEntity.SetMovementState(MovementState.SlowDown);
                        }
                        else
                        {
                            aiEntity.SetMovementState(MovementState.SpeedUp);
                        }
                    }
                    else
                    {
                        aiEntity.SetMovementState(MovementState.SpeedUp);
                    }
                    break;
                }
            case AIState.Waiting:
                {
                    switch (aiEntity.GetCurrentCrossingZone.CrossableType)
                    {
                        case CrossableType.CrossWalk:
                            {
                                if (ShouldAskForCross())
                                {
                                    AskForCross();
                                }
                                else
                                {
                                    if (HasTurnForCrossing())
                                    {
                                        if (IsAnotherEntityAskingForCross() && ShouldGiveCross())
                                        {
                                            GiveCross();
                                        }
                                        else
                                        {
                                            StartCross();
                                        }
                                    }
                                }
                                break;
                            }
                        case CrossableType.Intersection:
                            {
                                if (HasTurnForCrossing())
                                {
                                    StartCross();
                                }
                                break;
                            }
                    }
                    break;
                }
            case AIState.WaitingAndAsking:
                {
                    if (canCrossAfterWait)
                    {
                        if (!ShouldWaitForClearCross() || HasTurnForCrossing())
                        {
                            StartCross();
                            //aiEntity.CheckIfIsBreakingTheLaw();
                        }
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// Checks using a raycast if there is a obstacle in front of the entity.
    /// </summary>
    /// <returns></returns>
    private RaycastCheckResult IsThereAObstacle()
    {
        RaycastCheckResult obstacleCheckResult = default;
        if (aiEntity.IsOnTheStreet /* && !aiEntity.IsCrossingCrosswalk */)
        {
            obstacleCheckResult = aiEntity.CheckForObstacles();
        }
        return obstacleCheckResult;
    }

    /// <summary>
    /// Should the entity avoid a posible collision.
    /// </summary>
    /// <returns></returns>
    private bool ShouldAvoidCollision()
    {
        bool avoid = true;
        float chanceForAvoidingCollision = UnityEngine.Random.Range(0, 1.0f) * 100.0f;
        if (chanceForAvoidingCollision < 100 - avoidCollisionProbability)
        {
            avoid = false;
        }
        return avoid;
    }

    private bool HasTurnForCrossing()
    {
        bool canCross = true;
        if (aiEntity.GetCurrentCrossingZone != null)
        {
            canCross = aiEntity.GetCurrentCrossingZone.CanCross(aiEntity);
        }
        return canCross;
    }

    public void OnCrossWalkEntered()
    {
        DebugController.LogMessage("Waiting at crosswalk");
        aiEntity.SwitchToState(AIState.Waiting);
    }

    public void OnIntersectionEntered()
    {
        DebugController.LogMessage("Waiting at intersection");
        aiEntity.SwitchToState(AIState.Waiting);
    }

    /// <summary>
    /// Use to run a probality to see if the ai should ask for cross in the crosswalk.
    /// </summary>
    /// <returns></returns>
    private bool ShouldAskForCross()
    {
        bool askForCross = false;
        if (!alreadyGaveCross)
        {
            float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
            if (randomNumber >= 100 - askForCrossProbability)
            {
                askForCross = true;
            }
        }
        return askForCross;
    }

    /// <summary>
    /// Use to run a probality to see if the ai should allow another ai cross if it's asking for cross.
    /// </summary>
    /// <returns></returns>
    private bool ShouldGiveCross()
    {
        bool giveCross = false;
        if (!alreadyGaveCross)
        {
            float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
            if (randomNumber >= 100 - giveCrossProbability)
            {
                giveCross = true;
            }
        }
        return giveCross;
    }

    /// <summary>
    /// Use to run a probality to see if the ai should allow another ai cross if it's asking for cross.
    /// </summary>
    /// <returns></returns>
    private bool ShouldWaitForClearCross()
    {
        float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
        bool waitForClear = false;
        if (randomNumber >= 100 - waitForClearCrossProbability)
        {
            waitForClear = true;
        }
        return waitForClear;
    }

    private IEnumerator AskedForCrossWait()
    {
        yield return askedForCrossWait;
        canCrossAfterWait = true;
    }

    private void AskForCross()
    {
        DebugController.LogMessage($"{ gameObject.name } is asking for cross!");
        aiEntity.SwitchToState(AIState.WaitingAndAsking);
        canCrossAfterWait = false;
        StartCoroutine(AskedForCrossWait());
        onStartedAskingForCross?.Invoke(aiEntity.GetCurrentDirection);
    }

    private bool IsAnotherEntityAskingForCross()
    {
        EntityType otherEntity = (aiEntity.GetEntityType.Equals(EntityType.Car)) ? EntityType.Pedestrian : EntityType.Car;
        bool asking = false;
        if (aiEntity.GetCurrentCrossingZone is Crosswalk)
        {
            asking = ((Crosswalk)aiEntity.GetCurrentCrossingZone).IsThereAEntityAskingForCross(otherEntity);
        }
        return asking;
    }

    private void StartCross()
    {
        aiEntity.OnStartedCrossing(aiEntity.GetCurrentCrossingZone);
        // aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
        // aiEntity.SwitchToState(AIState.Moving);
        alreadyGaveCross = false;
        //aiEntity.CheckIfIsBreakingTheLaw();
    }

    private void GiveCross()
    {
        if (aiEntity.GetCurrentCrossingZone != null && aiEntity.GetCurrentCrossingZone is Crosswalk)
        {
            ((Crosswalk)aiEntity.GetCurrentCrossingZone).OnEntityGivingCross(aiEntity);
            alreadyGaveCross = true;
        }
    }
}
