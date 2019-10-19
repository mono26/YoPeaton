using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController
{
    private static bool debugActive = true;
    private static float drawDebugDuration = 10.0f;

    /// <summary>
    /// Logs a raw message without formating.
    /// </summary>
    /// <param name="message"> Raw message to log (Must come already in format).</param>
    public static void LogMessage(string message) {
        if (debugActive) {
            Debug.Log(message);
        }
    }

    public static void DrawDebugRay(Vector3 _origin, Vector3 _direction, float _lenght, Color _color) {
        if (debugActive) {
            // Debug.DrawRay(_origin, _direction * _lenght, _color, drawDebugDuration);
        }
    }
}
