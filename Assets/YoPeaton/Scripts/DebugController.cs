using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController
{
    [SerializeField]
    private static bool debugActive = true;

    /// <summary>
    /// Logs a raw message without formating.
    /// </summary>
    /// <param name="message"> Raw message to log (Must come already in format).</param>
    public static void LogMessage(string message) {
        if (debugActive) {
            Debug.Log(message);
        }
    }
}
