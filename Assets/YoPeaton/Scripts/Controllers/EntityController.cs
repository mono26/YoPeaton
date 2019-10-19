using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField]
    private IMovable movableComponent;
    [SerializeField]
    private FollowPath followComponent;

    float distanceTravelled = 0.0f;
    private bool move = true;

    protected IMovable GetMovableComponent {
        get {
            if (movableComponent == null) {
                movableComponent = GetComponent<IMovable>();
                if (movableComponent == null) {
                    DebugController.LogMessage(string.Format("Gameobject {0} doesn't have IMovable component", gameObject.name));
                }
            }
            return movableComponent;
        }
    }

    protected FollowPath GetFollowPathComponent {
        get {
            if (followComponent == null) {
                followComponent = GetComponent<FollowPath>();
                if (followComponent == null) {
                    DebugController.LogMessage(string.Format("Gameobject {0} doesn't have FollowPath component", gameObject.name));
                }
            }
            return followComponent;
        }
    }

    private void Start() {
        GetInitialValuesToStartPath();
    }

    private void GetInitialValuesToStartPath() {
        float timeOnCurrentPath = followComponent.GetTParameter(transform.position);
        distanceTravelled = followComponent.GetLengthAt(timeOnCurrentPath);
    }

    private void FixedUpdate() {
        if (ShouldStop()) {
            DebugController.LogMessage("STOP!");
            GetMovableComponent?.SlowDown();
        }
        else if (ShouldSlowDown()) {
            DebugController.LogMessage("Slowing down");
            GetMovableComponent?.SlowDown();
        }
        else {
            GetMovableComponent?.SpeedUp();
        }
        if (GetFollowPathComponent) {
            distanceTravelled += GetMovableComponent.GetCurrentSpeed * Time.fixedDeltaTime;
            if (move) {
                float t = distanceTravelled / GetFollowPathComponent.GetPathLeght;
                GetMovableComponent?.MoveToPosition(GetFollowPathComponent.GetPosition(t));
            }
        }
    }

    protected abstract bool ShouldStop();

    protected abstract bool ShouldSlowDown();

    protected virtual void OnTriggerEnter2D(Collider2D _other) {
        if (_other.CompareTag("ChangeOfDirection")) {
            DirectionChange directionChanger = _other.GetComponent<DirectionChange>();
            DirectionPathPair[] conections = directionChanger.Getconections;
            BezierSpline nextPath = conections[Random.Range(0, conections.Length)].path;
            followComponent.SetPath = nextPath;
            GetInitialValuesToStartPath();
            DebugController.LogMessage("Got new path");
        }
    }
}
