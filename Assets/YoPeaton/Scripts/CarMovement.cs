using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D carBody = null;
    [SerializeField]
    private float maxSpeed = 0.0f;
    [SerializeField]
    private float brakeSpeed = 0.0f;
    [SerializeField]
    private float acceleration = 0.0f;

    private float currentSpeed = 0.0f;
    private bool isBraking = false;

    private void Update() {
        if (Input.GetKey(KeyCode.B)) {
            isBraking = true;
        }
        else {
            isBraking = false;
        }
    }

    private void FixedUpdate() {
        if (isBraking) {
            ApplyBrakes();
        }
        else {
            Accelerate();
        }
        MoveToNextPosition();
    }

    private void Accelerate() {
        if (currentSpeed < maxSpeed) {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (currentSpeed >  maxSpeed) {
            currentSpeed = maxSpeed;
        }
    }

    private void MoveToNextPosition() {
        Vector3 currentPosition = carBody.position;
        Vector3 nextPosition = currentPosition + (Vector3.right * currentSpeed * Time.deltaTime);
        carBody.MovePosition(nextPosition);
    }

    public void ApplyBrakes() {
        if (currentSpeed > 0.0f) {
            currentSpeed -= brakeSpeed * Time.deltaTime;
        }
    }
}
