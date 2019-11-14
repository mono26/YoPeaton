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
    private TextMeshProUGUI currentDirection;
    [SerializeField]
    private TextMeshProUGUI canCross;
    [SerializeField]
    private TextMeshProUGUI onTheStreet;
    [SerializeField]
    private TextMeshProUGUI obstacle;
    [SerializeField]
    private AIController aiToDebug;

    private void Start() {
        if (!DebugController.debugActive) {
            gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (DebugController.debugActive) {
            if (speedText)
            {
                speedText.text = aiToDebug.GetMovableComponent.GetCurrentSpeed.ToString();
            }
            if (currentState)
            {
                currentState.text = aiToDebug.GetCurrentState.ToString();
            }
            if (canCross)
            {
                canCross.text = "Cross: " + aiToDebug.GetCurrentCrossingZone?.CanCross(aiToDebug).ToString();
            }
            
            if (onTheStreet)
            {
                onTheStreet.text = "Street: " + aiToDebug.IsOnTheStreet.ToString();
            }
            if (currentDirection)
            {
                currentDirection.text = "Direction: " + aiToDebug.GetCurrentDirection.ToString();
            }
            if (obstacle)
            {
                obstacle.text = "Obstacle: " + aiToDebug.IsThereAObstacleUpFront().ToString();
            }
        }
    }
}
