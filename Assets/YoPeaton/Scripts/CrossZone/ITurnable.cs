using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnable
{
    TurnInfo CurrentTurn { get; }

    /// <summary>
    /// Changes the currento turn to a new turn.
    /// </summary>
    /// <param name="_type">Type of entity the turn is for.</param>
    /// <param name="_duration">Duration in seconds of the turn.</param>
    void ChangeTurn(EntityType _type, float _duration);

    /// <summary>
    /// Changes the currento turn to a new turn.
    /// </summary>
    /// <param name="_type">Type of entity the turn is for.</param>
    void ChangeTurn(EntityType _type);

    /// <summary>
    /// Check if the currento turn is for the entity.
    /// </summary>
    /// <param name="_type">Type of entity to see if has the turn.</param>
    bool HasTurn(EntityType _type);

    /// <summary>
    /// Updates the current turn.
    /// </summary>
    /// <param name="_deltaTime"></param>
    void UpdateTurn(float _deltaTime);
}
