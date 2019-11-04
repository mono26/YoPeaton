using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField]
    private IMovable movableComponent;
    [SerializeField]
    private FollowPath followComponent;
    [SerializeField]
    private AnimatorController animationComponent;


    [SerializeField]
    private float changeDirectionProbability = 50.0f;
    [SerializeField]
    private EntityTypes type;
    //private EntityTypes entityType;

    private float distanceTravelled = 0.0f;
    private float lastTPArameter = 0.0f;
    private bool move = true;
    [SerializeField]
    private bool isOnTheStreet = false;



    public bool IsOnTheStreet {
        get {
            return isOnTheStreet;
        }
    }

    public IMovable GetMovableComponent {
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

    public EntityTypes GetEntityType {
        get {
            return type;
        }
    }

    private void Start() {
        SetEntityType();
        animationComponent = this.GetComponent<AnimatorController>();
        //animationComponent.SetAnimator(type);
        animationComponent.SetCurrentAnimation(followComponent.GetDirection(Time.time));
        GetInitialValuesToStartPath();


    }

    private void SetEntityType()
    {
        float probability = Random.Range(0f, 1f);

        if(gameObject.tag == "Pedestrian")
        {
            if (probability < 0.5f)
            {
                type = EntityTypes.Male;
            } else if (probability < 1f)
            {
                type = EntityTypes.Female;
            }
        }

        if (gameObject.tag == "Car")
        {
            if(probability < 0.65f)
            {
                type = EntityTypes.BlueCar;
            } else
            {
                type = EntityTypes.YellowCar;
            }
        }
    }

    private void GetInitialValuesToStartPath() {
        float timeOnCurrentPath = followComponent.GetTParameter(transform.position);
        distanceTravelled = followComponent.GetLengthAt(timeOnCurrentPath);
        lastTPArameter = timeOnCurrentPath;
    }

    protected virtual void FixedUpdate() {
        if (ShouldStop()) {
            // DebugController.LogMessage("STOP!");
            GetMovableComponent?.SlowDown();
        }
        else if (ShouldSlowDown()) {
            // DebugController.LogMessage("Slowing down");
            GetMovableComponent?.SlowDown();
        }
        else {
            GetMovableComponent?.SpeedUp();
        }
        if (GetFollowPathComponent) {
            distanceTravelled += GetMovableComponent.GetCurrentSpeed * Time.fixedDeltaTime;
            if (move) {
                float t;
                if (distanceTravelled / GetFollowPathComponent.GetPathLeght < lastTPArameter) {
                    t = lastTPArameter;
                }
                else {
                    t = distanceTravelled / GetFollowPathComponent.GetPathLeght;
                }
                if (!t.Equals(lastTPArameter)) {
                    GetMovableComponent?.MoveToPosition(GetFollowPathComponent.GetPosition(t));
                    lastTPArameter = t;
                }
            }
        }
    }

    protected abstract bool ShouldStop();

    protected abstract bool ShouldSlowDown();

    protected virtual void OnTriggerEnter2D(Collider2D _other) {
        if (_other.CompareTag("ChangeOfDirection")) {
            float chanceOfChangingDirection = 100.0f;
            if (!followComponent.IsTheEndOfPath(_other.transform.position)) {
                chanceOfChangingDirection = Random.Range(0, 1.0f) * 100.0f;
            }
            if (chanceOfChangingDirection >= 100 - changeDirectionProbability) {
                //Debug.LogError("Nombre: " + this.gameObject.name + ", Changed Direction.");
                DirectionChange directionChanger = _other.GetComponent<DirectionChange>();
                BezierSpline newPath = directionChanger.GetConnectionFrom(followComponent.GetPath);
                if (newPath) {
                    if (animationComponent != null)
                    {
                        animationComponent.SetCurrentAnimation(newPath.GetDirection(Time.time));
                    }
                    followComponent.SetPath = newPath;
                    GetInitialValuesToStartPath();
                    movableComponent.SlowDown(50.0f);
                }
            }
        }
        else if (_other.CompareTag("StreetBounds")) {
            isOnTheStreet = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D _other) {
        if (_other.CompareTag("StreetBounds")) {
            isOnTheStreet = false;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D _other) {
        if (!isOnTheStreet) {
            if (_other.CompareTag("StreetBounds")) {
                isOnTheStreet = true;
            }
        }
    }
}
