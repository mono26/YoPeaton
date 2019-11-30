using UnityEngine;

public interface IMovable 
{
    float GetCurrentSpeed { get; }
    EntityController GetEntity { get; }
    void SpeedUp(float _deltaTime);
    void MoveToPosition(Vector3 nextPosition);
    void AddOnMovement(System.Action<OnEntityMovementEventArgs> _onMovementAction);
    void RemoveOnMovement(System.Action<OnEntityMovementEventArgs> _onMovementAction);
}
