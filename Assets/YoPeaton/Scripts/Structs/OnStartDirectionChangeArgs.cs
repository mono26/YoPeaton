using System.Collections;
using UnityEngine;

public struct OnStartDirectionChangeArgs
{
    public Vector3 Direction { get; set; }
    public Path NextPath { get; set; }
}
