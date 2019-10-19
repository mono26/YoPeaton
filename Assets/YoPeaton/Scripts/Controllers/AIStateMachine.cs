using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    Moving, SlowDown, StopAtCrossWalk
}
public class AIStateMachine : MonoBehaviour
{
    [SerializeField]
    private AIState currentState;

    public AIState GetCurrentState {
        get {
            return currentState;
        }
    }

    public void SwitchToState(AIState _newState) {
        currentState = _newState;
    }
}
