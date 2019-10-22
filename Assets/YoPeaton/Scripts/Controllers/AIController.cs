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

    public Crosswalk GetCurrentCorosingZone {
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

    protected override void OnTriggerEnter2D(Collider2D _other) {
        base.OnTriggerEnter2D(_other);
        if (_other.CompareTag("HotZone")) {
            Crosswalk crossWalk = _other.transform.parent.parent.GetComponent<Crosswalk>();
            if (currentCrossingZone) {
                // If we entered first a hot zone and then a crosswalk, and both crosswalks references are different. Something weird just happend.
                if (!currentCrossingZone.Equals(crossWalk)) {
                    DebugController.LogMessage("Whoops something weird just happend...");
                    currentCrossingZone = crossWalk;
                    transitionController.OnCrossWalkEntered();
                }
            }
            else {
                currentCrossingZone = crossWalk;
                transitionController.OnCrossWalkEntered();
            }
        }
        if (_other.CompareTag("CrossWalk")) {
            Crosswalk crossWalk = _other.transform.parent.GetComponent<Crosswalk>();
            if (currentCrossingZone) {
                // If we entered first a hot zone and then a crosswalk, and both crosswalks references are different. Something weird just happend.
                if (!currentCrossingZone.Equals(crossWalk)) {
                    DebugController.LogMessage("Whoops something weird just happend...");
                    currentCrossingZone = crossWalk;
                    transitionController.OnCrossWalkEntered();
                }
            }
            else {
                currentCrossingZone = crossWalk;
                transitionController.OnCrossWalkEntered();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _other) {
        if (_other.CompareTag("CrossWalk")) {
            if (currentCrossingZone) {
                Crosswalk crossWalk = _other.transform.parent.GetComponent<Crosswalk>();
                if (crossWalk && currentCrossingZone.Equals(crossWalk)) {
                    currentCrossingZone = null;
                }
            }
        }
    }

    public void SwitchToState(AIState _newState) {
        stateMachine.SwitchToState(_newState);
    }

    protected override bool ShouldStop() {
        return GetCurrentState.Equals(AIState.StopAtCrossWalk);
    }

    protected override bool ShouldSlowDown() {
        return GetCurrentState.Equals(AIState.SlowDown);
    }
}
