using System;
using UnityEngine;

public class EntityMovement : MonoBehaviour, IMovable, ISlowable
{
    private Action<OnEntityMovementEventArgs> onMovement;
    private Action<EntityController> onAccelerate;
    private Action<EntityController> onBrake;

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
        if (GetEntity)
        {
            direction = GetEntity.GetFollowPathComponent.GetDirection(Time.time);
        }
        else
        {
            direction = GetComponent<EntityFollowPath>().GetDirection(Time.time);
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
        onAccelerate?.Invoke(GetEntity);
        // ToggleBrakeLights(false);
    }

    private void MoveToNextPosition(Vector3 _position) {
        _position.z = 0;
        direction = GetEntity.GetFollowPathComponent.GetDirection(Time.time);
        carBody.MovePosition(_position);
        OnEntityMovementEventArgs eventArgs = new OnEntityMovementEventArgs();
        eventArgs.Entity = movingEntity;
        eventArgs.MovementDirection = direction; 
        onMovement?.Invoke(eventArgs);
    }

    private void RotateInDirectionOfPosition(Vector3 _position) {
        Vector3 directionTowardsPosition = (_position - transform.position).normalized;
    }

    public void ApplyBrakes(float _deltaTime) {
        if (currentSpeed > 0.0f) {
            currentSpeed -= brakeSpeed * _deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, maxSpeed);
        }
        onBrake?.Invoke(GetEntity);
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

    public void SlowDownByPercent(float _slowPercent) {
        currentSpeed = currentSpeed * ((100 - _slowPercent) / 100);
        onBrake?.Invoke(GetEntity);
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

    public void AddOnAccelerate(Action<EntityController> _onMovementAction)
    {
        onAccelerate += _onMovementAction;
    }

    public void RemoveOnAccelerate(Action<EntityController> _onMovementAction)
    {
        onAccelerate -= _onMovementAction;
    }

    public void AddOnBrake(Action<EntityController> _onMovementAction)
    {
        onBrake += _onMovementAction;
    }

    public void RemoveOnBrake(Action<EntityController> _onMovementAction)
    {
        onBrake -= _onMovementAction;
    }
}
