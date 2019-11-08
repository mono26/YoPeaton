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
    [SerializeField]
    private float maxDistanceToCheckForStop = 3.0f;
    [SerializeField]
    private LayerMask layersToCheckCollision;

    private float distanceTravelled = 0.0f;
    private float lastTPArameter = 0.0f;
    private bool move = true;
    private bool isOnTheStreet = false;
    private float colliderRadius;
    private Vector3 colliderOffset;

#region Properties
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
#endregion

    //Awake is always called before any Start functions
    protected virtual void Awake()
    {
        var collider = GetComponent<CircleCollider2D>();
        colliderRadius = collider.radius;
        colliderOffset = collider.offset;
        SetEntityType();
        animationComponent = this.GetComponent<AnimatorController>();
        //animationComponent.SetAnimator(type);
        if (animationComponent) {
            animationComponent.SetCurrentAnimation(followComponent.GetDirection(Time.time));
        }
    }

    private void Start() {
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

    protected virtual void Update() 
    {
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
    }

    protected virtual void FixedUpdate() {
        if (GetFollowPathComponent && GetMovableComponent.GetCurrentSpeed > 0.0f) {
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

    public abstract void OnCrossWalkEntered(Crosswalk _crossWalk);

    public abstract void OnCrossWalkExited(Crosswalk _crossWalk);

    /// <summary>
    /// Checks for a obstacle ahead.true If it's a pedestrian or car stops.
    /// </summary>
    /// <returns>True if there is a obstacle ahead. False if not.</returns>
    public bool IsThereAObstacleUpFront() {
        bool stop = false;
        if (IsOnTheStreet && !IsCrossingACrossWalk()) {
            // if (gameObject.CompareTag("Pedestrian") && IsCrossingACrossWalk()) {
            //     //
            // }
            // else {
            //     // Wee have to calculate new distance and starposition values because we want to avoid detecting our selfs.
            //     Vector3 startPosition = transform.position + (transform.right * (float)(((colliderRadius) * transform.localScale.x) + 0.1));
            //     float distance = maxDistanceToCheckForStop - ((colliderRadius * 2) * transform.localScale.x);
            //     float checkWidth = ((colliderRadius + colliderRadius/2) * 2) * transform.localScale.x;
            //     GameObject obstacle = PhysicsHelper.RayCastOverALineForFirstGameObject(gameObject, startPosition, transform.up, checkWidth, transform.right, distance, layersToCheckCollision, 5);
            //     if (obstacle) {
            //         if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car")) {
            //             if (obstacle.GetComponent<EntityController>().IsOnTheStreet) {
            //                 stop = true;
            //             }
            //         }
            //     }
            // }
            // Wee have to calculate new distance and starposition values because we want to avoid detecting our selfs.
            Vector3 startPosition = transform.position + colliderOffset + (transform.right * (float)(((colliderRadius) * transform.localScale.x) + 0.1));
            float distance = maxDistanceToCheckForStop - ((colliderRadius * 2) * transform.localScale.x);
            float checkWidth = ((colliderRadius + colliderRadius/4) * 2) * transform.localScale.x;
            GameObject obstacle = PhysicsHelper.RayCastOverALineForFirstGameObject(gameObject, startPosition, transform.up, checkWidth, transform.right, distance, layersToCheckCollision, 5);
            if (obstacle) {
                if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car")) {
                    if (obstacle.GetComponent<EntityController>().IsOnTheStreet) {
                        stop = true;
                    }
                }
            }
        }
        return stop;
    }

    public abstract bool IsCrossingACrossWalk();
}
