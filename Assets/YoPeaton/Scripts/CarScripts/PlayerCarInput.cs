using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarInput : MonoBehaviour
{
    [SerializeField]
    private bool isBraking = false;

    public bool IsBraking {
        get {
            return isBraking;
        }
    }

    private void Update() {
        CheckBrakeInput();
    }

    private void CheckBrakeInput()
    {
        if (Input.GetMouseButton(0)) {
            isBraking = true;
        }
        else {
            isBraking = false;
        }
    }
}
