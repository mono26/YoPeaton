using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarInput : MonoBehaviour
{
    [SerializeField]
    public bool IsBraking { get; private set; }
    [SerializeField]
    public bool IsAccelerating { get; private set; }

    public void Brake()
    {
        IsBraking = true;
        Debug.Log("Is Braking");
        //return isBraking;
    }

    public void StopBrake()
    {
        IsBraking = false;
        Debug.Log("Stop Braking");
        //return isBraking;
    }

    public void Accelerate()
    {
        DebugController.LogMessage("Is accelerating!");
        IsAccelerating = true;
    }

    public void StopAccelerate()
    {
        IsAccelerating = false;
    }
}
