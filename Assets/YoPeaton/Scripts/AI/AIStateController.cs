using System;
using System.Collections;
using UnityEngine;

public class AIStateController : MonoBehaviour
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
    private Coroutine canCrossWait;
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

    public void UpdateState()
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
                    OnCrossing();
                    break;
                }
            case AIState.Moving:
                {
                    OnMoving();
                    break;
                }
            case AIState.Waiting:
                {
                    OnWaiting();
                    break;
                }
            case AIState.WaitingAndAsking:
                {
                    OnWaitingAndAsking();
                    break;
                }
        }
    }

    #region State Actions
    private void OnCrossing()
    {
        OnMoving();
        //if (ShouldAvoidCollision())
        //{
        //    RaycastCheckResult result = IsThereAObstacle();
        //    if (result.collided && result.otherEntity.IsCrossingCrosswalk)
        //    {
        //        aiEntity.SetMovementState(MovementState.SlowDown);
        //    }
        //    else
        //    {
        //        aiEntity.SetMovementState(MovementState.SpeedUp);
        //    }
        //    // Speed up
        //}
        //else
        //{
        //    aiEntity.SetMovementState(MovementState.SpeedUp);
        //}
    }

    private void OnMoving()
    {
        if (ShouldAvoidCollision())
        {
            if (IsThereAObstacle())
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
    }

    private void OnWaiting()
    {
        if (ShouldWaitForClearCross())
        {
            switch (aiEntity.GetCurrentCrossingZone.CrossableType)
            {
                case CrossableType.CrossWalk:
                    {
                        // Can run probaility for should wait for clear pass.
                        if (ShouldAskForCross())
                        {
                            AskForCross();
                        }
                        else
                        {
                            if (HasTurnForCrossing())
                            {
                                if (aiEntity.GetEntityType.Equals(EntityType.Vehicle))
                                {
                                    if ((IsAnotherEntityAskingForCross() || IsAnotherEntityWaiting()) && ShouldGiveCross())
                                    {
                                        GiveCross();
                                    }
                                    else
                                    {
                                        StartCross();
                                    }
                                }
                                else
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
        }
        else
        {
            StartCross();
        }
    }

    private void OnWaitingAndAsking()
    {
        if (canCrossAfterWait)
        {
            if (!ShouldWaitForClearCross() || HasTurnForCrossing())
            {
                StartCross();
                //aiEntity.CheckIfIsBreakingTheLaw();
            }
        }
    }
    #endregion

    #region Conditions
    /// <summary>
    /// Checks using a raycast if there is a obstacle in front of the entity.
    /// </summary>
    /// <returns></returns>
    private bool IsThereAObstacle()
    {
        bool obstacle = false;
        RaycastCheckResult obstacleCheckResult = default;
        if (aiEntity.IsOnTheStreet)
        {
            obstacleCheckResult = aiEntity.HasAObstacleUpFront();
            if (obstacleCheckResult.collided && obstacleCheckResult.otherEntity.IsOnTheStreet)
            {
                obstacle = true;
            }
        }
        return obstacle;
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

    /// <summary>
    /// Use to run a probality to see if the ai should ask for cross in the crosswalk.
    /// </summary>
    /// <returns></returns>
    private bool ShouldAskForCross()
    {
        bool askForCross = false;
        if (!alreadyGaveCross)
        {
            //if (!((Crosswalk)aiEntity.GetCurrentCrossingZone).IsTurnInCooldown(aiEntity.GetEntityType))
            //{
            //    float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
            //    if (randomNumber >= 100 - askForCrossProbability)
            //    {
            //        askForCross = true;
            //    }
            //}
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

    private bool IsAnotherEntityAskingForCross()
    {
        EntityType otherEntity = (aiEntity.GetEntityType.Equals(EntityType.Vehicle)) ? EntityType.Pedestrian : EntityType.Vehicle;
        bool asking = false;
        if (aiEntity.GetCurrentCrossingZone is Crosswalk)
        {
            asking = ((Crosswalk)aiEntity.GetCurrentCrossingZone).IsThereAEntityAskingForCross(otherEntity);
        }
        return asking;
    }

    private bool IsAnotherEntityWaiting()
    {
        EntityType otherEntity = (aiEntity.GetEntityType.Equals(EntityType.Vehicle)) ? EntityType.Pedestrian : EntityType.Vehicle;
        bool waiting = false;
        if (aiEntity.GetCurrentCrossingZone is Crosswalk)
        {
            waiting = ((Crosswalk)aiEntity.GetCurrentCrossingZone).IsThereAEntityWaiting(otherEntity);
        }
        return waiting;
    }
    #endregion

    private IEnumerator AskedForCrossWait()
    {
        yield return askedForCrossWait;
        canCrossAfterWait = true;
    }

    #region Transitions
    private void AskForCross()
    {
        // DebugController.LogMessage($"{ gameObject.name } is asking for cross!");
        aiEntity.SwitchToState(AIState.WaitingAndAsking);
        canCrossAfterWait = false;
        canCrossWait = StartCoroutine(AskedForCrossWait());
        onStartedAskingForCross?.Invoke(aiEntity.GetCurrentDirection);
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

    public void OnCrossWalkEntered()
    {
        DebugController.LogMessage($"Waiting at crosswalk { gameObject.name }");
        aiEntity.SwitchToState(AIState.Waiting);
    }

    public void OnIntersectionEntered()
    {
        DebugController.LogMessage("Waiting at intersection");
        aiEntity.SwitchToState(AIState.Waiting);
    }
    #endregion
}
