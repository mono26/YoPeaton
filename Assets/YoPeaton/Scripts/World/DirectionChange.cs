using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions {
	North, East, West, South
}

[System.Serializable]
public struct DirectionPathPair {
	public Directions direction;
	public BezierSpline path;

	public DirectionPathPair(Directions _direction, BezierSpline _path) {
		direction = _direction;
		path = _path;
	}
}

public class DirectionChange : MonoBehaviour
{
    [SerializeField]
    private DirectionPathPair[] connections = null;

    public DirectionPathPair[] Getconections {
        get {
            return connections;
        }
    }

    private void OnDrawGizmos() {
        if (connections.Length > 0) {
            for (int i = 0; i < connections.Length; i++) {
                if (connections[i].path) {
                    float t = connections[i].path.GetTParameter(transform.position);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(connections[i].path.GetPoint(t), transform.position);
                }
            }
        }
    }
}
