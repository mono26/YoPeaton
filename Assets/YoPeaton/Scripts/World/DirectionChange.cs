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
}
