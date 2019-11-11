using UnityEngine;

public class AIController : EntityController {
    [SerializeField]
    private AIStateMachine stateMachine = null;
    [SerializeField]
    private AITransitionsController transitionController;
    [SerializeField]
    private Crosswalk currentCrossingZone;
    [SerializeField]
    private InfractionController behaviourController;

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



    protected override void Awake() {
        base.Awake();
        // Catching the transitions controller.
        if (!transitionController) {
            transitionController = GetComponent<AITransitionsController>();
            if (!transitionController) {
                transitionController = gameObject.AddComponent<AITransitionsController>();
            }
        }
        transitionController.SetController = this;
        // Catching the state machine.
        if (!stateMachine) {
            stateMachine = GetComponent<AIStateMachine>();
            if (!stateMachine) {
                stateMachine = gameObject.AddComponent<AIStateMachine>();
            }
        }
    }

    protected override void Update() {
        transitionController.CheckTransitions();
        base.Update();
    }

    protected override void FixedUpdate() {
        // First check posible transitions.
        base.FixedUpdate();
    }

    public void SwitchToState(AIState _newState) {
        stateMachine.SwitchToState(_newState);
    }

    protected override bool ShouldStop() {
        // Si esta esperando en un crosswalk
        bool stop = false;
        if (GetCurrentState.Equals(AIState.WaitingAtCrossWalkAndAskingForPass)) {
            stop = true;
        }
        else if (GetCurrentState.Equals(AIState.WaitingAtCrossWalk)) {
            stop = true;
        }
        return stop;
    }

    protected override bool ShouldSlowDown() {
        // Si esta por collisionar
        return GetCurrentState.Equals(AIState.SlowDown);
    }

    public bool getShouldStop()
    {
        return GetCurrentState.Equals(AIState.WaitingAtCrossWalk);
    }

    public bool getShouldSlowDown()
    {
        return GetCurrentState.Equals(AIState.SlowDown);
    }

    public override void OnCrossWalkEntered(Crosswalk _crossWalk) {
        // DebugController.LogMessage("Entered crosswalk");
        currentCrossingZone = _crossWalk;
        transitionController.OnCrossWalkEntered();
    }

    public override void OnCrossWalkExited(Crosswalk _crossWalk) {
        // DebugController.LogMessage("Exited crosswalk");
        if (currentCrossingZone.Equals(_crossWalk)) {
            currentCrossingZone = null;
            SwitchToState(AIState.Moving);
        }
    }

    public void CheckIfIsBreakingTheLaw() {
        if(currentCrossingZone != null)
        behaviourController?.CheckAllInfractions(currentCrossingZone);
    }

    public override bool IsCrossingACrossWalk() {
        return (GetCurrentCrossingZone != null && GetCurrentState.Equals(AIState.CrossingCrossWalk));
    }
}
