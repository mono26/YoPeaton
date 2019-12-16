using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrossWalkDebug : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI turnText;
    [SerializeField]
    private Crosswalk crosswalkToDebug;

    private void Start()
    {
        if (!DebugController.debugActive)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (DebugController.debugActive)
        {
            if (turnText)
            {
                turnText.text = $"Current turn: { crosswalkToDebug.CurrentTurn.Type.ToString() }";
            }
        }
    }
}
