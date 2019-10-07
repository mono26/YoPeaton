using UnityEngine;

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
    private FollowPath followComponent = null;

    private float currentSpeed = 0.0f;
    private float progressInPath = 0.0f;
    private float pathLength = 0.0f;
    private bool isBraking = false;
    private bool move = true;

    private void Start() {
        pathLength = followComponent.GetLength();
        progressInPath = 0.0f;
    }

    private void OnEnable() {
        followComponent.OnPathCompleted(OnPathCompleted);
    }

    private void OnDisable() {
        followComponent.ClearFromPathCompleted(OnPathCompleted);
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
            float t = progressInPath / timeToFinishPath;
            DebugController.LogMessage(string.Format("t: {0}", t));
            // MoveWithDirection(followComponent.GetNextDirection(t));
            MoveToNextPosition(followComponent.GetNextPosition(t));
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
            currentSpeed -= brakeSpeed * Time.deltaTime;
        }
    }

    private void OnPathCompleted()
    {
        BezierSpline nextPath = followComponent.GetNextPosiblePath();
        if (nextPath) {
            pathLength = followComponent.GetLength();
            progressInPath = 0.0f;
        }
        else {
            
        }
        
    }
}
