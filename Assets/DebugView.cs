using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugView : MonoBehaviour
{
    [SerializeField]
    private bool enableDebug;

    private void Awake()
    {
        DebugController.debugActive = enableDebug;
    }
}
