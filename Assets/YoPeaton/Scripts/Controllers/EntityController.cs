using System.Collections;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
#region Dependencies
    [SerializeField]
    private IMovable movableComponent;
    [SerializeField]
    private FollowPath followComponent;
    [SerializeField]
    private AnimatorController animationComponent;
#endregion

#region Variables
    [SerializeField]
    private float changeDirectionProbability = 50.0f;
    [SerializeField]
    private EntitySubType entitySubType;
    [SerializeField]
    private float maxDistanceToCheckForStop = 3.0f;
    [SerializeField]
    private LayerMask layersToCheckCollision;
    [SerializeField]
    private EntityType entityType;
    [SerializeField]
    private float exitedCrosswalkClearTime = 3.0f;
#endregion

    private float distanceTravelled = 0.0f;
    private float lastTParameter = 0.0f;
    private bool move = true;
    private bool isOnTheStreet = false;
    private float colliderRadius;
    private Vector3 colliderOffset;
    private BezierSpline nextPath;
    private float nextPathStarting_t_Parameter;
    private float currentPathConnected_t_ParameterToNextPath;
    private bool isChangingDirection = false;
    private Crosswalk exitedCrosswalk;
    private WaitForSeconds exitedCrosswalkClearWait;
    protected bool entityIsPlayer = true;

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

    public EntitySubType GetEntitySubType {
        get {
            return entitySubType;
        }
    }

    public EntityType GetEntityType {
        get {
            return entityType;
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
        exitedCrosswalkClearWait = new WaitForSeconds(exitedCrosswalkClearTime);
        //animationComponent.SetAnimator(type);
    }

    private void Start() {
        GetInitialValuesToStartPath();
        if (animationComponent) {
            animationComponent.SetCurrentAnimation(followComponent.GetDirection(lastTParameter));
        }
    }

    private void SetEntityType()
    {
        float probability = Random.Range(0f, 1f);

        if(gameObject.tag == "Pedestrian")
        {
            if (probability < 0.5f)
            {
                entitySubType = EntitySubType.Male;
            } else if (probability < 1f)
            {
                entitySubType = EntitySubType.Female;
            }
        }

        if (gameObject.tag == "Car" && !entityIsPlayer)
        {
            if(probability < 0.4f)
            {
                entitySubType = EntitySubType.RedCar;
            } else if (probability < 0.65f)
            {
                entitySubType = EntitySubType.GreenCar;
            }
            else
            {
                entitySubType = EntitySubType.YellowCar;
            }
        } else if (entityIsPlayer)
        {
            entitySubType = EntitySubType.BlueCar;
        }
    }

    private void GetInitialValuesToStartPath() {
        float timeOnCurrentPath = followComponent.GetTParameter(transform.position);
        distanceTravelled = followComponent.GetLengthAt(timeOnCurrentPath);
        lastTParameter = timeOnCurrentPath;
    }

    protected virtual void Update() 
    {
        float deltaTime = Time.deltaTime;
        if (ShouldStop()) {
            // DebugController.LogMessage("STOP!");
            GetMovableComponent?.SlowDownByPercent(100.0f);
        }
        else if (ShouldSlowDown()) {
            // DebugController.LogMessage("Slowing down");
            GetMovableComponent?.SlowDown(deltaTime);
        }
        else {
            GetMovableComponent?.SpeedUp(deltaTime);
        }
    }

    protected virtual void FixedUpdate() {
        if (GetFollowPathComponent && GetMovableComponent.GetCurrentSpeed > 0.0f) {
            distanceTravelled += GetMovableComponent.GetCurrentSpeed * Time.fixedDeltaTime;
            if (move) {
                float t;
                if (distanceTravelled / GetFollowPathComponent.GetPathLeght < lastTParameter) {
                    t = lastTParameter;
                }
                else {
                    t = distanceTravelled / GetFollowPathComponent.GetPathLeght;
                }
                if (!t.Equals(lastTParameter)) {

                    if (isChangingDirection && nextPath) {
                        if (t >= currentPathConnected_t_ParameterToNextPath) {
                            transform.position = nextPath.GetPoint(nextPathStarting_t_Parameter);
                            // transform.right = nextPath.GetDirection(nextPathStarting_t_Parameter);
                            followComponent.SetPath = nextPath;
                            t = nextPathStarting_t_Parameter;
                            GetInitialValuesToStartPath();
                            if (entityType.Equals(EntityType.Car))
                            {
                                movableComponent.SlowDown(50.0f);
                            }
                            isChangingDirection = false;
                            nextPath = null;
                        }
                    }
                    GetMovableComponent?.MoveToPosition(GetFollowPathComponent.GetPosition(t));
                    lastTParameter = t;
                }
            }
        }
    }

    protected abstract bool ShouldStop();

    protected abstract bool ShouldSlowDown();

    protected virtual void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.CompareTag("ChangeOfDirection")) 
        {
            float chanceOfChangingDirection = 100.0f;
            if (!followComponent.IsTheEndOfPath(_other.transform.position)) 
            {
                chanceOfChangingDirection = Random.Range(0, 1.0f) * 100.0f;
            }
            if (chanceOfChangingDirection >= 100 - changeDirectionProbability) 
            {
                //Debug.LogError("Nombre: " + this.gameObject.name + ", Changed Direction.");
                DirectionChange directionChanger = _other.GetComponent<DirectionChange>();
                nextPath = directionChanger.GetConnectionFrom(followComponent.GetPath);
                if (nextPath) 
                {
                    nextPathStarting_t_Parameter = nextPath.GetTParameter(transform.position);
                    currentPathConnected_t_ParameterToNextPath = followComponent.GetPath.GetTParameter(nextPath.GetPoint(nextPathStarting_t_Parameter));
                    isChangingDirection = true;
                }
            }
        }
        else if (_other.CompareTag("StreetBounds"))
        {
            isOnTheStreet = true;
        }
        else if (_other.CompareTag("Car") || _other.gameObject.CompareTag("Pedestrian"))
        {
            EntityController otherEntity = _other.GetComponent<EntityController>();
            DebugController.LogErrorMessage(string.Format("Collided with other entity {0}", otherEntity.gameObject.name));
            // Collision with entity.
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

    public virtual void OnCrossWalkExited(Crosswalk _crossWalk)
    {
        exitedCrosswalk = _crossWalk;
        StartCoroutine(ClearExitedCrossWalk());
    }

    /// <summary>
    /// Checks for a obstacle ahead.true If it's a pedestrian or car stops.
    /// </summary>
    /// <returns>True if there is a obstacle ahead. False if not.</returns>
    public bool IsThereAObstacleUpFront() 
    {
        bool stop = false;
        Vector3 direction = (GetFollowPathComponent.GetPosition(lastTParameter + 0.1f) - GetFollowPathComponent.GetPosition(lastTParameter)).normalized;
        Vector3 startPosition = transform.position + colliderOffset + (direction * (float)(((colliderRadius) * transform.localScale.x) + 0.1f));
        float distance = maxDistanceToCheckForStop - ((colliderRadius * 2) * transform.localScale.x);
        float checkWidth = ((colliderRadius + colliderRadius/4) * 2) * transform.localScale.x;
        Vector3 axis = Vector3.Cross(direction, Vector3.forward);
        GameObject obstacle = PhysicsHelper.RayCastOverALineForFirstGameObject(gameObject, startPosition, axis, checkWidth, direction, distance, layersToCheckCollision, 5);
        if (obstacle) {
            if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car")) 
            {
                //if (obstacle.GetComponent<EntityController>().IsOnTheStreet) 
                //{
                //    stop = true;
                //}
                stop = true;
            }
        }
        return stop;
    }

    public bool JustExitedCrossWalk(Crosswalk _crossWalk)
    {
        bool justExited = false;
        if (exitedCrosswalk)
        {
            if (exitedCrosswalk.Equals(_crossWalk))
            {
                justExited = true;
            }
        }
        return justExited;
    }

    private IEnumerator ClearExitedCrossWalk()
    {
        yield return exitedCrosswalkClearWait;
        exitedCrosswalk = null;
    }
}
