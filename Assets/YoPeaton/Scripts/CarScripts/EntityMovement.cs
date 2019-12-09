﻿using System;
using UnityEngine;

public class EntityMovement : MonoBehaviour, IMovable, ISlowable
{
    private Action<OnEntityMovementEventArgs> onMovement;

    [SerializeField]
    private EntityController movingEntity;
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
    public Vector3 direction;

    [SerializeField]
    private GameObject carLightsVFX;

    public float GetCurrentSpeed {
        get {
            return currentSpeed;
        }
    }

    public float GetMaxSpeed
    {
        get
        {
            return maxSpeed;
        }
    }

    public EntityController GetEntity {
        get {
            return movingEntity;
        }
    }

    private void Start()
    {
        this.transform.localRotation = new Quaternion(0, 0, 0, 0);
        ToggleBrakeLights(false);
        if (GetEntity)
        {
            direction = GetEntity.GetFollowPathComponent.GetDirection(Time.time);
        }
        else
        {
            direction = GetComponent<FollowPath>().GetDirection(Time.time);
        }
        OnEntityMovementEventArgs eventArgs = new OnEntityMovementEventArgs();
        eventArgs.Entity = movingEntity;
        eventArgs.MovementDirection = direction;
        onMovement?.Invoke(eventArgs);
    }

    private void Accelerate(float _deltaTime) {
        if (currentSpeed < maxSpeed) {
            currentSpeed += acceleration * _deltaTime;
        }
        else if (currentSpeed >  maxSpeed) {
            currentSpeed = maxSpeed;
        }
        ToggleBrakeLights(false);
    }

    private void MoveWithDirection(float _detltaTime, Vector3 _direction) {
        Vector3 currentPosition = carBody.position;
        Vector3 nextPosition = currentPosition + (_direction.normalized * currentSpeed * _detltaTime);
        MoveToNextPosition(nextPosition);
    }

    private void MoveToNextPosition(Vector3 _position) {
        // Vector3 lerpedPosition = Vector2.Lerp(transform.position, _position, Time.fixedDeltaTime);
        // carBody.MovePosition(lerpedPosition);
        // RotateInDirectionOfPosition(lerpedPosition);
        _position.z = 0;
        direction = GetEntity.GetFollowPathComponent.GetDirection(Time.time);
        //direction = (_position - transform.position).normalized;
        //Debug.LogError(direction);
        carBody.MovePosition(_position);
        //RotateInDirectionOfPosition(_position);
        // currentDirection = (lerpedPosition - transform.position).normalized;
        // onMovement?.Invoke(currentDirection);
        // currentDirection = (_position - transform.position).normalized;
        OnEntityMovementEventArgs eventArgs = new OnEntityMovementEventArgs();
        eventArgs.Entity = movingEntity;
        eventArgs.MovementDirection = direction; 
        onMovement?.Invoke(eventArgs);
    }

    private void RotateInDirectionOfPosition(Vector3 _position) {
        Vector3 directionTowardsPosition = (_position - transform.position).normalized;
        // bool isOpositeDirection = Vector3.Dot(transform.right, directionTowardsPosition) < 0;
        // if (!isOpositeDirection) {
        //     Vector2 targetRotation = Vector2.Lerp((Vector2)transform.right, directionTowardsPosition, Time.fixedDeltaTime);
        //     transform.right = targetRotation;
        // }
        // Vector2 lerpedRotation = Vector2.Lerp((Vector2)transform.right, directionTowardsPosition, Time.fixedDeltaTime);
        // transform.right = lerpedRotation;
        //transform.right = directionTowardsPosition;
    }

    public void ApplyBrakes(float _deltaTime) {
        if (currentSpeed > 0.0f) {
            currentSpeed -= brakeSpeed * _deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, maxSpeed);
        }
        ToggleBrakeLights(true);
    }

    private void ApplyInmediateStop()
    {
        currentSpeed = 0;
    }

    public void ShouldInmediatlyStop()
    {
        ApplyInmediateStop();
    }
    public void SpeedUp(float _deltaTime) {
        Accelerate(_deltaTime);
    }

    public void SlowDown(float _deltaTime) {
        // Clamp to a min value.
        ApplyBrakes(_deltaTime);
        
    }

    private void ToggleBrakeLights(bool status)
    {
        if (GetEntity.GetEntityType == EntityType.Car)
        {
            if (status && direction == new Vector3(0, 1, 0))
            {
                carLightsVFX.SetActive(true);
            }
            else
            {
                carLightsVFX.SetActive(false);
            }
        }
            
    }

    public void SlowDownByPercent(float _slowPercent) {
        currentSpeed = currentSpeed * ((100 - _slowPercent) / 100);
        ToggleBrakeLights(true);
    }

    public void MoveToPosition(Vector3 position) {
        MoveToNextPosition(position);
    }

    public void AddOnMovement(Action<OnEntityMovementEventArgs> _onMovementAction) {
        onMovement += _onMovementAction;
    }

    public void RemoveOnMovement(Action<OnEntityMovementEventArgs> _onMovementAction) {
        onMovement -= _onMovementAction;
    }
}
