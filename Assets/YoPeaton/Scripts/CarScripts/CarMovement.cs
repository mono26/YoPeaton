﻿using UnityEngine;

public class CarMovement : MonoBehaviour, IMovable
{
    [SerializeField]
    private Rigidbody2D carBody = null;
    [SerializeField]
    private float maxSpeed = 0.0f;
    [SerializeField]
    private float brakeSpeed = 0.0f;
    [SerializeField]
    private float acceleration = 0.0f;

    [SerializeField]
    private float currentSpeed = 0.0f;
    private bool isBraking = false;
    private bool move = true;

    public float GetCurrentSpeed {
        get {
            return currentSpeed;
        }
    }

    private void Update() {
        if (Input.GetKey(KeyCode.B)) {
            isBraking = true;
        }
        else {
            isBraking = false;
        }
    }

    // private void FixedUpdate() {
    //     if (isBraking) {
    //         ApplyBrakes();
    //     }
    //     else {
    //         Accelerate();
    //     }
    //     float timeToFinishPath = pathLength / currentSpeed;
    //     if (float.IsNaN(timeToFinishPath))
    //     {
    //         timeToFinishPath = 0.0f;
    //     }
    //     if (move)
    //     {
    //         float t = progressInPath / timeToFinishPath;
    //         progressInPath += Time.fixedDeltaTime;
    //         // MoveWithDirection(followComponent.GetNextDirection(t));
    //         Vector3 nextPosition = Vector3.Lerp(transform.position, followComponent.GetNextPosition(t), t);
    //         Debug.DrawRay(nextPosition, Vector3.right, Color.red, 10.0f);
    //         Debug.DrawRay(nextPosition, Vector3.up, Color.red, 10.0f);
    //         Debug.Log(t);
    //         MoveToNextPosition(nextPosition);
    //     }
    // }

    private void Accelerate() {
        if (currentSpeed < maxSpeed) {
            currentSpeed += acceleration * Time.fixedDeltaTime;
        }
        else if (currentSpeed >  maxSpeed) {
            currentSpeed = maxSpeed;
        }
    }

    private void MoveWithDirection(Vector3 _direction) {
        Vector3 currentPosition = carBody.position;
        Vector3 nextPosition = currentPosition + (_direction * currentSpeed * Time.deltaTime);
        carBody.MovePosition(nextPosition);
    }

    private void MoveToNextPosition(Vector3 _position) {
        carBody.MovePosition(_position);
    }

    public void ApplyBrakes() {
        if (currentSpeed > 0.0f) {
            currentSpeed -= brakeSpeed * Time.fixedDeltaTime;
        }
    }

    public void SpeedUp() {
        Accelerate();
    }

    public void SlowDown() {
        ApplyBrakes();
    }

    public void MoveToPosition(Vector3 position) {
        MoveToNextPosition(position);
    }
}
