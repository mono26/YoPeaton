using System.Collections;
using UnityEngine;

public struct OnStartDirectionChangeArgs
{
    public Vector3 CurrentDirection { get; set; }
    public Vector3 NextDirection { get; set; }
    public Path NextPath { get; set; }
}
