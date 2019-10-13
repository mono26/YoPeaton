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
        Vector3 nextPosition = currentPosition + (_direction.normalized * currentSpeed * Time.fixedDeltaTime);
        MoveToNextPosition(nextPosition);
    }

    private void MoveToNextPosition(Vector3 _position) {
        carBody.MovePosition(_position);
        RotateInDirectionOfPosition(_position);
    }

    private void RotateInDirectionOfPosition(Vector3 _position) {
        Vector3 directionTowardsPosition = (_position - transform.position).normalized;
        Vector2 targetRotation = Vector2.Lerp((Vector2)transform.right, directionTowardsPosition, 0.5f);
        transform.right = targetRotation;
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
