using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AIDebug : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI speedText;
    [SerializeField]
    private TextMeshProUGUI currentState;
    [SerializeField]
    private TextMeshProUGUI canCross;
    [SerializeField]
    private TextMeshProUGUI onTheStreet;
    [SerializeField]
    private AIController aiToDebug;

    private void Start() {
        if (!DebugController.debugActive) {
            gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (DebugController.debugActive) {
            speedText.text = aiToDebug.GetMovableComponent.GetCurrentSpeed.ToString();
            currentState.text = aiToDebug.GetCurrentState.ToString();
            canCross.text = "Cross: " + aiToDebug.GetCurrentCrossingZone?.CanCross(aiToDebug).ToString();
            onTheStreet.text = "Street: " + aiToDebug.IsOnTheStreet.ToString();
        }
    }
}
