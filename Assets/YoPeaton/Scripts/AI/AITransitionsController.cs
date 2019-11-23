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
    private float stopProbability = 100.0f;
    [SerializeField]
    private float waitForClearCrossProbability = 100.0f;

#region Dependencies
    [SerializeField]
    private AIController aiEntity;
#endregion

    private bool alreadyGaveCross = false;
    private bool canCrossAfterWait = true;
    private RaycastCheckResult obstacleCheckResult;
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
        if (currentState.Equals(AIState.WaitingAtCrossWalkAndAskingForPass))
        {
            // TODO: Run probability of wait for clear pass?!
            if (canCrossAfterWait && aiEntity.GetCurrentCrossingZone)
            {
                if (!ShouldWaitForClearCross() || CanCrossCurrentCrossingZone())
                {
                    aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
                    aiEntity.SwitchToState(AIState.Moving);
                    //aiEntity.CheckIfIsBreakingTheLaw();
                }
            }
        }
        // Waiting to cross a crosswalk.
        else if (currentState.Equals(AIState.WaitingAtCrossWalk))
        {
            if (ShouldAskForCross())
            {
                aiEntity.SwitchToState(AIState.WaitingAtCrossWalkAndAskingForPass);
                canCrossAfterWait = false;
                StartCoroutine(AskedForCrossWait());
                onStartedAskingForCross?.Invoke(aiEntity.GetCurrentDirection);
                DebugController.LogMessage(string.Format("{0} asked for cross!", gameObject.name));
            }
            else if (aiEntity.GetCurrentCrossingZone && CanCrossCurrentCrossingZone())
            {
                // TODO: Run probability for letting an entity asking for pass cross?!
                EntityType otherEntity = (aiEntity.GetEntityType.Equals(EntityType.Car)) ? EntityType.Pedestrian : EntityType.Car;
                bool giveCross = ShouldGiveCross();
                bool isSomeoneAkingForCross = aiEntity.GetCurrentCrossingZone.IsThereAEntityAskingForCross(otherEntity);
                if (!alreadyGaveCross && isSomeoneAkingForCross && giveCross)
                {
                    aiEntity.GetCurrentCrossingZone.OnEntityGivingCross(aiEntity);
                    alreadyGaveCross = true;
                }
                else if (alreadyGaveCross || !giveCross || !isSomeoneAkingForCross)
                {
                    aiEntity.GetCurrentCrossingZone.OnStartedCrossing(aiEntity);
                    aiEntity.SwitchToState(AIState.Moving);
                    alreadyGaveCross = false;
                    //aiEntity.CheckIfIsBreakingTheLaw();
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

    private bool IsThereAObstacle()
    {
        bool isThereAObstacle = false;
        obstacleCheckResult = aiEntity.CheckForObstacles();
        if (aiEntity.IsOnTheStreet && obstacleCheckResult.collided)
        {
            if (obstacleCheckResult.otherEntity.IsOnTheStreet)
            {
                isThereAObstacle = true;
            }
        }
        return isThereAObstacle;
    }

    private bool ShouldAvoidCollision()
    {
        bool avoid = false;
        float chanceForAvoidingCollision = UnityEngine.Random.Range(0, 1.0f) * 100.0f;
        if (aiEntity.GetEntityType.Equals(EntityType.Car) && obstacleCheckResult.otherEntity.GetEntityType.Equals(EntityType.Pedestrian))
        {
            if (chanceForAvoidingCollision >= 100 - avoidCollisionProbability) 
            {
                avoid = true;
            }
        }
        return avoid;
    }

    private bool CanCrossCurrentCrossingZone()
    {
        bool canCross = true;
        if (aiEntity.GetCurrentCrossingZone)
        {
            canCross = aiEntity.GetCurrentCrossingZone.CanCross(aiEntity);
        }
        return canCross;
    }

    public void OnCrossWalkEntered() 
    {
        aiEntity.SwitchToState(AIState.WaitingAtCrossWalk);
    }

    /// <summary>
    /// Use to run a probality to see if the ai should ask for cross in the crosswalk.
    /// </summary>
    /// <returns></returns>
    private bool ShouldAskForCross()
    {
        float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
        bool askForPass = false;
        if (!aiEntity.GetCurrentCrossingZone.GetWaitingTicket(aiEntity).gaveCross)
        {
            if (randomNumber >= 100 - askForCrossProbability)
            {
                askForPass = true;
            }
        }
        return askForPass;
    }

    /// <summary>
    /// Use to run a probality to see if the ai should allow another ai cross if it's asking for cross.
    /// </summary>
    /// <returns></returns>
    private bool ShouldGiveCross()
    {
        float randomNumber = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
        bool giveCross = false;
        if (aiEntity.GetCurrentCrossingZone.GetWaitingTicket(aiEntity).gaveCross)
        {
            giveCross = true;
        }
        else if (randomNumber >= 100 - giveCrossProbability)
        {
            giveCross = true;
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
}
