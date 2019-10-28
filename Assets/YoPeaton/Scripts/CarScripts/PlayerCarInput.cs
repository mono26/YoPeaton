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

    public void Brake()
    {
        isBraking = true;
        Debug.Log("Is Braking");
        //return isBraking;
    }

    public void StopBrake()
    {
        isBraking = false;
        Debug.Log("Stop Braking");
        //return isBraking;
    }
    /*private void Update() {
        CheckBrakeInput();
    }

    private void CheckBrakeInput()
    {
        if (Input.GetMouseButton(0)) {
            Debug.Log("Is Braking");
            isBraking = true;
        }
        else {
            isBraking = false;
        }
    }*/
}
