using System;

public interface ISlowable
{
    /// <summary>
    /// Slows down.
    /// </summary>
    /// <param name="_deltaTime"></param>
    void SlowDown(float _deltaTime);

    /// <summary>
    /// Lower the current speed to a percentage.
    /// </summary>
    /// <param name="_slowPercent">Percentage to reduce speed to.</param>
    void SlowDownByPercent(float _slowPercent);

    void ShouldInmediatlyStop();

    void AddOnBrake(Action<EntityController> _onMovementAction);

    void RemoveOnBrake(Action<EntityController> _onMovementAction);
}
