using System;
using UnityEngine;

public interface IMovable 
{
    EntityController GetEntity { get; }
    float GetCurrentSpeed { get; }

    void SpeedUp(float _deltaTime);

    void MoveToPosition(Vector3 nextPosition);

    void AddOnMovement(Action<OnEntityMovementEventArgs> _onMovementAction);

    void RemoveOnMovement(Action<OnEntityMovementEventArgs> _onMovementAction);

    void AddOnAccelerate(Action<EntityController> _onMovementAction);

    void RemoveOnAccelerate(Action<EntityController> _onMovementAction);
}
