using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChange : MonoBehaviour
{
    [SerializeField]
    private PathConnectionPair[] connection = null;

    private void OnDrawGizmos()
    {
        if (connection.Length > 0)
        {
            for (int i = 0; i < connection.Length; i++)
            {
                if (connection[i].from && connection[i].to)
                {
                    float t = connection[i].from.GetTParameter(transform.position);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(connection[i].from.GetPointAt(t), transform.position);
                    float t2 = connection[i].to.GetTParameter(transform.position);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(connection[i].to.GetPointAt(t2), transform.position);
                }
            }
        }
    }

    public Path GetConnectionFrom(Path _pathToGetNexTConnection)
    {
        List<Path> posibleConnections = new List<Path>();
        for (int i = 0; i < connection.Length; i++)
        {
            if (connection[i].IsConnectionForPath(_pathToGetNexTConnection))
            {
                posibleConnections.Add(connection[i].GetConnection(_pathToGetNexTConnection));
            }
        }
        Path connectionToReturn = null;
        if (posibleConnections.Count > 0)
        {
            connectionToReturn = posibleConnections[Random.Range(0, posibleConnections.Count)];
        }
        return connectionToReturn;
    }

    public bool HasConnection(Path _pathToCheck)
    {
        bool hasConnection = false;
        foreach (PathConnectionPair connectionPair in connection)
        {
            if (connectionPair.IsConnectionForPath(_pathToCheck))
            {
                hasConnection = true;
                break;
            }
        }
        return hasConnection;
    }
}
