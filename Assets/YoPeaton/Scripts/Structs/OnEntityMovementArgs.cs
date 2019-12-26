using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OnEntityMovementEventArgs
{
    public EntityController Entity { get; set; }
    public Vector3 MovementDirection { get; set; }
}