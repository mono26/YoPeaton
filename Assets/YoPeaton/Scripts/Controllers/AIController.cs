using UnityEngine;

public class AIController : EntityController {
    [SerializeField]
    private AIStateMachine stateMachine = null;
    [SerializeField]
    private AITransitionsController transitionController;
    [SerializeField]
    private Crosswalk currentCrossingZone;

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

    private void Awake() {
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

    protected override void FixedUpdate() {
        // First check posible transitions.
        transitionController.CheckTransitions();
        base.FixedUpdate();
    }

    public void SwitchToState(AIState _newState) {
        stateMachine.SwitchToState(_newState);
    }

    protected override bool ShouldStop() {
        return GetCurrentState.Equals(AIState.WaitingAtCrossWalk);
    }

    protected override bool ShouldSlowDown() {
        return GetCurrentState.Equals(AIState.SlowDown);
    }

    public void OnCrossWalkEntered(Crosswalk _crossWalk) {
        DebugController.LogMessage("Entered crosswalk");
        currentCrossingZone = _crossWalk;
        transitionController.OnCrossWalkEntered();
    }

    public void OnCrossWalkExited(Crosswalk _crossWalk) {
        DebugController.LogMessage("Exited crosswalk");
        if (currentCrossingZone.Equals(_crossWalk)) {
            currentCrossingZone = null;
            SwitchToState(AIState.Moving);
        }
    }
}
