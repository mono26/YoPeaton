using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable {
    float GetCurrentSpeed { get; }
    void SpeedUp();
    void MoveToPosition(Vector3 nextPosition);
    void SlowDown();
}
