using System;
using UnityEngine;

public class CarMovement : MonoBehaviour, IMovable
{
    private System.Action<Vector3> onMovement;

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
    public Vector3 currentDirection;

    [SerializeField]
    private GameObject carLightsVFX;

    public float GetCurrentSpeed {
        get {
            return currentSpeed;
        }
    }

    public EntityController GetEntity {
        get {
            return movingEntity;
        }
    }

    private void Start()
    {
        
            ToggleBrakeLights(false);
    }

    private void Accelerate() {
        if (currentSpeed < maxSpeed) {
            currentSpeed += acceleration * Time.fixedDeltaTime;
        }
        else if (currentSpeed >  maxSpeed) {
            currentSpeed = maxSpeed;
        }
        ToggleBrakeLights(false);
    }

    private void MoveWithDirection(Vector3 _direction) {
        Vector3 currentPosition = carBody.position;
        Vector3 nextPosition = currentPosition + (_direction.normalized * currentSpeed * Time.fixedDeltaTime);
        MoveToNextPosition(nextPosition);
    }

    private void MoveToNextPosition(Vector3 _position) {
        // Vector3 lerpedPosition = Vector2.Lerp(transform.position, _position, Time.fixedDeltaTime);
        // carBody.MovePosition(lerpedPosition);
        // RotateInDirectionOfPosition(lerpedPosition);
        carBody.MovePosition(_position);
        //RotateInDirectionOfPosition(_position);

        // currentDirection = (lerpedPosition - transform.position).normalized;
        // onMovement?.Invoke(currentDirection);
        currentDirection = (_position - transform.position).normalized;
        onMovement?.Invoke(currentDirection);
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
        transform.right = directionTowardsPosition;
    }

    public void ApplyBrakes() {
        if (currentSpeed > 0.0f) {
            currentSpeed -= brakeSpeed * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, maxSpeed);
        }
        ToggleBrakeLights(true);
    }

    public void SpeedUp() {
        Accelerate();
    }

    public void SlowDown() {
        // Clamp to a min value.
        ApplyBrakes();
        
    }

    private void ToggleBrakeLights(bool status)
    {
        if (GetEntity.GetEntityType == EntityTypes.BlueCar || GetEntity.GetEntityType == EntityTypes.YellowCar)
        {
            if (status && GetEntity.GetCurrentDirection == new Vector3(0, 1, 0))
            {
                carLightsVFX.SetActive(true);
            }
            else
            {
                carLightsVFX.SetActive(false);
            }
        }
            
    }

    public void SlowDown(float _slowPercent) {
        currentSpeed = currentSpeed * (_slowPercent / 100);
        ToggleBrakeLights(true);
    }

    public void SlowToStop() {
        // Clamp to zero.
        ApplyBrakes();
    }

    public void MoveToPosition(Vector3 position) {
        MoveToNextPosition(position);
    }

    public void AddOnMovement(Action<Vector3> _onMovementAction) {
        onMovement += _onMovementAction;
    }

    public void RemoveOnMovement(Action<Vector3> _onMovementAction) {
        onMovement -= _onMovementAction;
    }
}
