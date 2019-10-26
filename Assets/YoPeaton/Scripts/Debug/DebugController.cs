using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController
{
    public static bool debugActive = true;
    public static float drawDebugDuration = 10.0f;

    /// <summary>
    /// Logs a raw message without formating.
    /// </summary>
    /// <param name="message"> Raw message to log (Must come already in format).</param>
    public static void LogMessage(string message) {
        if (debugActive) {
            Debug.Log(message);
        }
    }

    /// <summary>
    /// Logs a raw message without formating.
    /// </summary>
    /// <param name="message"> Raw message to log (Must come already in format).</param>
    public static void LogErrorMessage(string message) {
        if (debugActive) {
            Debug.Log(string.Format("<color=red>{0}</color>",message));
        }
    }

    /// <summary>
    /// Logs a raw message without formating.
    /// </summary>
    /// <param name="message"> Raw message to log (Must come already in format).</param>
    public static void LogWarningMessage(string message) {
        if (debugActive) {
            Debug.Log(string.Format("<color=yellow>{0}</color>",message));
        }
    }

    public static void DrawDebugRay(Vector3 _origin, Vector3 _direction, float _lenght, Color _color) {
        if (debugActive) {
            Debug.DrawRay(_origin, _direction * _lenght, _color, drawDebugDuration);
        }
    }

    public static void DrawDebugLine(Vector3 _origin, Vector3 _endPosition, Color _color) {
        if (debugActive) {
            Debug.DrawLine(_origin, _endPosition, _color, drawDebugDuration);
        }
    }
}
