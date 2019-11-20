using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChange : MonoBehaviour
{
    [SerializeField]
    private PathConnectionPair[] connection = null;

    private void OnDrawGizmos() {
        if (connection.Length > 0) {
            for (int i = 0; i < connection.Length; i++) {
                float t = connection[i].from.GetTParameter(transform.position);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(connection[i].from.GetPoint(t), transform.position);
                float t2 = connection[i].to.GetTParameter(transform.position);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(connection[i].to.GetPoint(t2), transform.position);
            }
        }
    }

    public BezierSpline GetConnectionFrom(BezierSpline _pathToGetNexTConnection) {
        List<BezierSpline> posibleConnections = new List<BezierSpline>();
        for (int i = 0; i < connection.Length; i++) {
            if (connection[i].IsConnectionForPath(_pathToGetNexTConnection)) {
                posibleConnections.Add(connection[i].GetConnection(_pathToGetNexTConnection));
            }
        }
        BezierSpline connectionToReturn = null;
        if (posibleConnections.Count > 0) {
            connectionToReturn = posibleConnections[Random.Range(0, posibleConnections.Count)];
        }
        return connectionToReturn;
    }

    public bool  HasConnection(BezierSpline _pathToCheckForConnection)
    {
        bool hasConnection = false;
        foreach (PathConnectionPair connectionPair in connection) {
            if (connectionPair.IsConnectionForPath(_pathToCheckForConnection)) {
                hasConnection = true;
                break;
            }
        }
        return hasConnection;
    }
}
