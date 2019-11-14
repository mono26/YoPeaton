using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugController
{
    public static bool debugActive = false;
    public static float drawDebugDuration = 3.0f;
    public static int circleSteps = 50;

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

    public static void DrawDebugArrow(Vector3 _origin, Vector3 _endPosition, Color _color) {
        if (debugActive) {
            Vector3 arrowDirection = (_endPosition - _origin).normalized;
            Quaternion arrowAngle1 = Quaternion.Euler(0, 0, 45.0f);
            Quaternion arrowAngle2 = Quaternion.Euler(0, 0, -45.0f);
            Vector3 side1 = arrowAngle1 * -arrowDirection;
            Vector3 side2 = arrowAngle2 * -arrowDirection;
            Debug.DrawLine(_origin, _endPosition, _color, drawDebugDuration);
            DrawDebugRay(_endPosition, side1, 1.0f, _color);
            DrawDebugRay(_endPosition, side2, 1.0f, _color);
        }
    }

    public static void DrawDebugCircle(Vector3 _center, float _radius, Color _color) {
        float x;
        float y;
        float angleIncrement = 360.0f / (float)circleSteps;
        float angle = 0.0f;
        Vector3 previousPoint = _center;
        Vector3 nextPoint = _center;
        for (int i = 0; i < circleSteps; i++) {
            angle = angleIncrement * i;
            x = Mathf.Cos(angle);
            y = Mathf.Sin(angle);
            nextPoint = _center + new Vector3(x, y);
            DrawDebugLine(previousPoint, nextPoint, _color);
            previousPoint = nextPoint;
        }
    }
}
