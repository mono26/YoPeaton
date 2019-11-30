using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PathConnectionPair
{
    [SerializeField]
    public Path from;
    [SerializeField]
    public Path to;

    public bool IsConnectionForPath(Path _pathToCheck) {
        return (_pathToCheck == from);
    }

    public Path GetConnection(Path _pathToCheck) {
        return to;
    }
}
