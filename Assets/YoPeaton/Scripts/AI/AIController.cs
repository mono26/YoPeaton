using UnityEngine;

public abstract class AIController : EntityController
{
    [SerializeField]
    private AIStateMachine stateMachine = null;
    [SerializeField]
    private AIStateController transitionController;
    [SerializeField]
    private ICrossable currentCrossingZone;
    [SerializeField]
    private InfractionController behaviourController;

    private MovementState currentMovementState;

    public bool IsThisOnTheStreet;

    public AIState GetCurrentState {
        get {
            return stateMachine.GetCurrentState;
        }
    }

    public ICrossable GetCurrentCrossingZone {
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

    #region Unity functions
    protected override void Awake() 
    {
        base.Awake();
        // Catching the transitions controller.
        if (!transitionController)
        {
            transitionController = GetComponent<AIStateController>();
            if (!transitionController)
            {
                transitionController = gameObject.AddComponent<AIStateController>();
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
        transitionController?.UpdateState();
        base.Update();
    }
    #endregion

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
    
    public override void OnCrossWalkEntered(ICrossable _crossWalk)
    {
        DebugController.LogMessage("Entered crosswalk");
        if (currentCrossingZone == null || !currentCrossingZone.Equals(_crossWalk))
        {
            currentCrossingZone?.OnFinishedCrossing(this);
            currentCrossingZone = _crossWalk as Crosswalk;
            transitionController?.OnCrossWalkEntered();
        }
    }

    public override void OnCrossWalkExited(ICrossable _crossWalk)
    {
        base.OnCrossWalkExited(_crossWalk);
        // DebugController.LogMessage("Exited crosswalk");
        if (currentCrossingZone != null)
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
        if (currentCrossingZone is Crosswalk)
        {
            behaviourController?.CheckAllInfractions(currentCrossingZone as Crosswalk);
        }
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

    public override void OnStartedCrossing(ICrossable _crossable)
    {
        base.OnStartedCrossing(_crossable);
        GetCurrentCrossingZone.OnStartedCrossing(this);
        SwitchToState(AIState.Crossing);
    }

    public void SetMovementState(MovementState _state)
    {
        currentMovementState = _state;
    }

    public override void OnIntersectionEntered(ICrossable _intersection)
    {
        DebugController.LogMessage("Entered crosswalk");
        if (currentCrossingZone == null || !currentCrossingZone.Equals(_intersection))
        {
            currentCrossingZone?.OnFinishedCrossing(this);
            currentCrossingZone = _intersection as Crosswalk;
            transitionController?.OnIntersectionEntered();
        }
    }

    public override void OnIntersectionExited(ICrossable _intersection)
    {
        // base.OnIntersectionExited(_intersection);
        // DebugController.LogMessage("Exited crosswalk");
        if (currentCrossingZone != null)
        {
            if (currentCrossingZone.Equals(_intersection))
            {
                currentCrossingZone = null;
            }
        }
        SwitchToState(AIState.Moving);
    }
}
