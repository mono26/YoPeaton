using System.Collections;
using UnityEngine;

public struct OnEntityStartDirectionChangeArgs
{
    public EntityController Entity { get; set; }
    public Vector3 Direction { get; set; }
    public Path NextPath { get; set; }
}
