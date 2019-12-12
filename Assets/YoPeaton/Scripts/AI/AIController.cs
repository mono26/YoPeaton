using UnityEngine;

public class AIController : EntityController
{
    [SerializeField]
    private AIStateMachine stateMachine = null;
    [SerializeField]
    private AITransitionsController transitionController;
    [SerializeField]
    private Crosswalk currentCrossingZone;
    [SerializeField]
    private InfractionController behaviourController;

    private MovementState currentMovementState;

    public bool IsThisOnTheStreet;

    public AIState GetCurrentState {
        get {
            return stateMachine.GetCurrentState;
        }
    }

    public Crosswalk GetCurrentCrossingZone {
        get {
            return currentCrossingZone;
        }
    }

    public Vector3 GetCurrentDirection
    {
        get {
            return this.GetFollowPathComponent.GetDirection(Time.time);
        }
    }

    protected override void Awake() 
    {
        IsThisOnTheStreet = base.IsOnTheStreet;
        entityIsPlayer = false;
        base.Awake();
        base.SetEntityType();

       
        // Catching the transitions controller.
        if (!transitionController)
        {
            transitionController = GetComponent<AITransitionsController>();
            if (!transitionController)
            {
                transitionController = gameObject.AddComponent<AITransitionsController>();
            }
        }
        transitionController.SetController = this;
        // Catching the state machine.
        if (!stateMachine)
        {
            stateMachine = GetComponent<AIStateMachine>();
            if (!stateMachine)
            {
                stateMachine = gameObject.AddComponent<AIStateMachine>();
            }
        }
    }

    protected override void Update()
    {
        IsThisOnTheStreet = base.IsOnTheStreet;
        transitionController?.CheckTransitions();
        base.Update();
    }

    public void SwitchToState(AIState _newState)
    {
        stateMachine?.SwitchToState(_newState);
    }

    protected override bool ShouldStop()
    {
        // Si esta esperando en un crosswalk
        bool stop = false;
        if (GetCurrentState.Equals(AIState.WaitingAndAsking)) 
        {
            stop = true;
        }
        else if (GetCurrentState.Equals(AIState.Waiting)) 
        {
            stop = true;
        }
        return stop;
    }

    protected override bool ShouldSlowDown()
    {
        bool slowDown = false;
        if (GetCurrentState.Equals(AIState.Moving) || GetCurrentState.Equals(AIState.Crossing))
        {
            slowDown = currentMovementState.Equals(MovementState.SlowDown);
        }
        return slowDown;
    }
    
    public override void OnCrossWalkEntered(Crosswalk _crossWalk)
    {
        DebugController.LogMessage("Entered crosswalk");
        if (!currentCrossingZone || !currentCrossingZone.Equals(_crossWalk))
        {
            currentCrossingZone?.OnFinishedCrossing(this);
            currentCrossingZone = _crossWalk;
            transitionController?.OnCrossWalkEntered();
        }
    }

    public override void OnCrossWalkExited(Crosswalk _crossWalk)
    {
        base.OnCrossWalkExited(_crossWalk);
        // DebugController.LogMessage("Exited crosswalk");
        if (currentCrossingZone)
        {
            if (currentCrossingZone.Equals(_crossWalk)) 
            {
                currentCrossingZone = null;
            }
        }
        SwitchToState(AIState.Moving);
    }

    public void CheckIfIsBreakingTheLaw()
    {
        //if(currentCrossingZone != null)
        behaviourController?.CheckAllInfractions(currentCrossingZone);
    }

    protected override bool ShouldSpeedUp()
    {
        bool speedUp = false;
        if (GetCurrentState.Equals(AIState.Moving) || GetCurrentState.Equals(AIState.Crossing))
        {
            speedUp = currentMovementState.Equals(MovementState.SpeedUp);
        }
        return speedUp;
    }

    public override void OnStartedCrossing()
    {
        base.OnStartedCrossing();
        GetCurrentCrossingZone.OnStartedCrossing(this);
        SwitchToState(AIState.Crossing);
    }

    public void SetMovementState(MovementState _state)
    {
        currentMovementState = _state;
    }
}
