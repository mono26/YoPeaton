using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChange : MonoBehaviour
{
    [SerializeField]
    private PathConnectionPair[] connections = null;

    private void OnDrawGizmos() {
        if (connections.Length > 0) {
            for (int i = 0; i < connections.Length; i++) {
                float t = connections[i].path1.GetTParameter(transform.position);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(connections[i].path1.GetPoint(t), transform.position);
                float t2 = connections[i].path2.GetTParameter(transform.position);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(connections[i].path2.GetPoint(t2), transform.position);
            }
        }
    }

    public BezierSpline GetConnectionFrom(BezierSpline _pathToGetNexTConnection) {
        List<BezierSpline> posibleConnections = new List<BezierSpline>();
        for (int i = 0; i < connections.Length; i++) {
            if (connections[i].HasAMatchingPath(_pathToGetNexTConnection)) {
                posibleConnections.Add(connections[i].GetConnection(_pathToGetNexTConnection));
            }
        }
        BezierSpline connectionToReturn = null;
        if (posibleConnections.Count > 0) {
            connectionToReturn = posibleConnections[Random.Range(0, posibleConnections.Count)];
        }
        return connectionToReturn;
    }
}
