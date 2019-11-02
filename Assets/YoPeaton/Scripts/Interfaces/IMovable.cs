﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable {
    float GetCurrentSpeed { get; }
    EntityController GetEntity { get; }
    void SpeedUp();
    void MoveToPosition(Vector3 nextPosition);
    void SlowDown();
    /// <summary>
    /// Lower the current speed to a percentage.
    /// </summary>
    /// <param name="_slowPercent">Percentage to reduce speed to.</param>
    void SlowDown(float _slowPercent);
    void SlowToStop();
    void AddOnMovement(System.Action<Vector3> _onMovementAction);
    void RemoveOnMovement(System.Action<Vector3> _onMovementAction);
}
