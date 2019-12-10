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
        if (currentState.Equals(AIState.WaitingAtCrossWalkAskingForCross))
        {
            // TODO: Run probability of wait for clear pass?!
            if (canCrossAfterWait)
            {
                if (!ShouldWaitForClearCross() || HasTurnForCrossing())
                {
                    StartCross();
                    //aiEntity.CheckIfIsBreakingTheLaw();
                }
            }
        }
        // Waiting to cross a crosswalk.
        else if (currentState.Equals(AIState.WaitingAtCrossWalk))
        {
            if (HasTurnForCrossing())
            {
                // TODO: Run probability for letting an entity asking for pass cross?!
                if (IsAnotherEntityAskingForCross() && ShouldGiveCross())
                {
                    aiEntity.GetCurrentCrossingZone?.OnEntityGivingCross(aiEntity);
                    alreadyGaveCross = true;
                }
                else
                {
                    StartCross();
                }
            }
            else
            {
                if (ShouldAskForCross())
                {
                    AskForCross();
                    // DebugController.LogMessage(string.Format("{0} asked for cross!", gameObject.name));
                }
            }
        }
        // Is moving on the street or on the sidewalks.
        else
        {
            if (IsThereAObstacle() && ShouldAvoidCollision())
            {
                aiEntity.SwitchToState(AIState.SlowDown);
            }
            else
            {
                aiEntity.SwitchToState(AIState.Moving);
            }
        }
    }

    /// <summary>
    /// Checks using a raycast if there is a obstacle in front of the entity.
    /// </summary>
    /// <returns></returns>
    private bool IsThereAObstacle()
    {
        bool isThereAObstacle = false;
        if (aiEntity.IsOnTheStreet /* && !aiEntity.IsCrossingCrosswalk */)
        {
            RaycastCheckResult obstacleCheckResult = aiEntity.CheckForObstacles();
            if (obstacleCheckResult.collided && obstacleCheckResult.otherEntity.IsOnTheStreet)
            {
                isThereAObstacle = true;
            }
        }
        return isThereAObstacle;
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
        if (aiEntity.GetCurrentCrossingZone)
        {
            canCross = aiEntity.GetCurrentCrossingZone.HasTurn(aiEntity);
        }
        return canCross;
    }

    public void OnCrossWalkEntered() 
    {
        DebugController.LogMessage("Waiting at crosswalk");
        aiEntity.SwitchToState(AIState.WaitingAtCrossWalk);
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
        aiEntity.SwitchToState(AIState.WaitingAtCrossWalkAskingForCross);
        canCrossAfterWait = false;
        StartCoroutine(AskedForCrossWait());
        onStartedAskingForCross?.Invoke(aiEntity.GetCurrentDirection);
    }

    private bool IsAnotherEntityAskingForCross()
    {
        EntityType otherEntity = (aiEntity.GetEntityType.Equals(EntityType.Car)) ? EntityType.Pedestrian : EntityType.Car;
        return aiEntity.GetCurrentCrossingZone.IsThereAEntityAskingForCross(otherEntity);
    }

    private void StartCross()
    {
        aiEntity.OnStartedCrossing();
        // aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
        // aiEntity.SwitchToState(AIState.Moving);
        alreadyGaveCross = false;
        //aiEntity.CheckIfIsBreakingTheLaw();
    }
}
