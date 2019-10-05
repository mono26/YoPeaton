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
    [SerializeField]
    private BezierSpline pathToFollow = null;

    private float currentSpeed = 0.0f;
    private float progressInPath = 0.0f;
    private float pathLength = 0.0f;
    private bool isBraking = false;
    private bool move = true;

    private void Start() {
        pathLength = pathToFollow.GetLength();
    }
    private void Update() {
        if (Input.GetKey(KeyCode.B)) {
            isBraking = true;
        }
        else {
            isBraking = false;
        }
    }

    private void FixedUpdate() {
        if (transform.position.Equals(pathToFollow.GetControlPoint(pathToFollow.ControlPointCount - 1)))
        {
            move = false;
        }
        if (isBraking) {
            ApplyBrakes();
        }
        else {
            Accelerate();
        }
        float timeToFinishPath = pathLength / currentSpeed;
        if (float.IsNaN(timeToFinishPath))
        {
            timeToFinishPath = 0.0f;
        }
        if (move)
        {
            MoveToNextPosition(GetMovementDirection(progressInPath / timeToFinishPath));
            progressInPath += Time.fixedDeltaTime;
        }
    }

    private void Accelerate() {
        if (currentSpeed < maxSpeed) {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (currentSpeed >  maxSpeed) {
            currentSpeed = maxSpeed;
        }
    }

    private void MoveToNextPosition(Vector3 _direction) {
        Vector3 currentPosition = carBody.position;
        Vector3 nextPosition = currentPosition + (_direction * currentSpeed * Time.deltaTime);
        carBody.MovePosition(nextPosition);
    }

    public void ApplyBrakes() {
        if (currentSpeed > 0.0f) {
            currentSpeed -= brakeSpeed * Time.deltaTime;
        }
    }

    private Vector3 GetMovementDirection(float time)
    {
        return pathToFollow.GetDirection(time);
    }
}
