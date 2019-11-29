using System;
using System.Collections;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    public Action<Vector3> onStartChangingDirection;
    public Action onStopChangingDirection;
    public Action onEntityCollision;

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
    private float distanceToCheckForStop = 3.0f;
    [SerializeField]
    private float exitedCrosswalkClearTime = 3.0f;
    [SerializeField]
    private EntityType entityType;
    [SerializeField]
    private EntitySubType entitySubType;
    [SerializeField]
    private LayerMask layersToCheckCollision;
    #endregion

    private bool canTurn = true;
    private BezierSpline nextPath;
    private Crosswalk exitedCrosswalk;
    private float colliderRadius;
    private float connected_t_Parameter_ToNextPath;
    private float distanceTravelled = 0.0f;
    private float distanceToCheckForCollision = 0.1f;
    private float lastTParameter = 0.0f;
    private float nextPathStarting_t_Parameter;
    public bool IsOnTheStreet { get; private set; }
    private bool move = true;
    private bool isChangingDirection = false;
    protected bool entityIsPlayer = true;
    protected RaycastCheckResult collisionCheckResult;
    private Vector3 colliderOffset;
    private WaitForSeconds exitedCrosswalkClearWait;

#region Properties

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

    public BezierSpline GetCurrentPath
    {
        get
        {
            return followComponent.GetPath;
        }
    }

    public BezierSpline GetNextPath
    {
        get
        {
            return nextPath;
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
        distanceToCheckForCollision = colliderRadius + 0.1f;
        SetEntityType();
        animationComponent = this.GetComponent<AnimatorController>();
        exitedCrosswalkClearWait = new WaitForSeconds(exitedCrosswalkClearTime);
        //animationComponent.SetAnimator(type);
    }

    private void Start() {
        IsOnTheStreet = false;
        GetInitialValuesToStartPath();
        if (animationComponent) {
            animationComponent.OnMovement(followComponent.GetDirection(lastTParameter));
        }
    }

    private void SetEntityType()
    {
        float probability = UnityEngine.Random.Range(0f, 1f);

        if(gameObject.tag == "Pedestrian")
        {
            if (probability < 0.3f)
            {
                entitySubType = EntitySubType.Male;
            } else if (probability < 0.6f)
            {
                entitySubType = EntitySubType.Female;
            }
            else if (probability < 0.67f)
            {
                entitySubType = EntitySubType.MaleWithBaby;
            }
            else if (probability < 0.74f)
            {
                entitySubType = EntitySubType.FemaleWithBaby;
            }
            else if (probability < 0.84f)
            {
                entitySubType = EntitySubType.MaleWithDog;
            }
            else if (probability < 0.94f)
            {
                entitySubType = EntitySubType.FemaleWithDog;
            }
            else if (probability < 0.97f)
            {
                entitySubType = EntitySubType.MaleWithWalker;
            }
            else if (probability < 1f)
            {
                entitySubType = EntitySubType.FemaleWithWalker;
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

    private void GetInitialValuesToStartPath() 
    {
        float timeOnCurrentPath = followComponent.GetTParameter(transform.position);
        distanceTravelled = followComponent.GetLengthAt(timeOnCurrentPath);
        lastTParameter = timeOnCurrentPath;
    }

    protected virtual void Update() 
    {
        float deltaTime = Time.deltaTime;
        if (ShouldStop()) 
        {
            // DebugController.LogMessage("STOP!");
            GetMovableComponent?.SlowDownByPercent(100.0f);
        }
        else if (ShouldSlowDown()) 
        {
            // DebugController.LogMessage("Slowing down");
            GetMovableComponent?.SlowDown(deltaTime);
        }
        else 
        {
            GetMovableComponent?.SpeedUp(deltaTime);
        }
    }

    protected virtual void FixedUpdate() 
    {
        if (HasCollided())
        {
            OnEntityCollision();
        }
        else if (GetFollowPathComponent && GetMovableComponent.GetCurrentSpeed > 0.0f) 
        {
            distanceTravelled += GetMovableComponent.GetCurrentSpeed * Time.fixedDeltaTime;
            if (move) {
                float t;
                if (distanceTravelled / GetFollowPathComponent.GetPathLeght < lastTParameter) 
                {
                    t = lastTParameter;
                }
                else 
                {
                    t = distanceTravelled / GetFollowPathComponent.GetPathLeght;
                }
                if (!t.Equals(lastTParameter)) 
                {

                    if (isChangingDirection && nextPath) 
                    {
                        if (t >= connected_t_Parameter_ToNextPath) 
                        {
                            // transform.position = nextPath.GetPoint(nextPathStarting_t_Parameter);
                            // transform.right = nextPath.GetDirection(nextPathStarting_t_Parameter);
                            followComponent.SetPath = nextPath;
                            t = nextPathStarting_t_Parameter;
                            GetInitialValuesToStartPath();
                            if (entityType.Equals(EntityType.Car))
                            {
                                movableComponent.SlowDownByPercent(50.0f);
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

    public bool HasCollided()
    {
        bool collided = false;
        collisionCheckResult = CheckForCollision();
        if (IsOnTheStreet && collisionCheckResult.collided && collisionCheckResult.otherEntity.IsOnTheStreet && GetMovableComponent.GetCurrentSpeed != 0)
        {
            DebugController.LogMessage($"Llamar metodo para cambiar a animacion de atropellado, { gameObject.name } Atropello a: { collisionCheckResult.otherEntity.name }");
            onEntityCollision?.Invoke();
            collided = true;
        }
        return collided;
    }

    protected abstract bool ShouldStop();

    protected abstract bool ShouldSlowDown();

    protected virtual void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.CompareTag("ChangeOfDirection") && canTurn == true) 
        {
            canTurn = false;
            StartCoroutine(ChaneCanTurnValueCR());
            DirectionChange directionChanger = _other.GetComponent<DirectionChange>();
            if (directionChanger.HasConnection(followComponent.GetPath))
            {
                if (ShouldChangeDirection()) 
                {
                    //Debug.LogError("Nombre: " + this.gameObject.name + ", Changed Direction.");
                    TryChangeDirection(directionChanger);
                    // if (entityType.Equals(EntityType.Car))
                    // {
                    //     movableComponent.SlowDown(30.0f);
                    // }
                }
            }
        }
        else if (_other.CompareTag("StreetBounds"))
        {
            if (!IsOnTheStreet)
            {
                IsOnTheStreet = true;
            }
        }
        //else if (_other.CompareTag("Car") || _other.gameObject.CompareTag("Pedestrian"))
        //{
        //    EntityController otherEntity = _other.GetComponent<EntityController>();
        //    DebugController.LogErrorMessage(string.Format("Collided with other entity {0}", otherEntity.gameObject.name));
        //    // Collision with entity.
        //}
    }

    IEnumerator ChaneCanTurnValueCR()
    {
        yield return new WaitForSeconds(2f);
        canTurn = true;
    }

    private bool ShouldChangeDirection()
    {
        float chanceOfChangingDirection = 100.0f;
        bool changeDirection = false;
        if (!followComponent.IsTheEndOfPath(transform.position) && followComponent.IsThereOtherChangeOfDirection()) 
        {
            chanceOfChangingDirection = UnityEngine.Random.Range(0, 1.0f) * 100.0f;
        }
        if (chanceOfChangingDirection >= 100 - changeDirectionProbability)
        {
            changeDirection =  true;
        }
        return changeDirection; 
    }

    private void TryChangeDirection(DirectionChange _directionChanger)
    {
        nextPath = _directionChanger.GetConnectionFrom(followComponent.GetPath);
        if (nextPath) 
        {
            nextPathStarting_t_Parameter = nextPath.GetTParameter(transform.position);
            connected_t_Parameter_ToNextPath = followComponent.GetPath.GetTParameter(nextPath.GetPoint(nextPathStarting_t_Parameter));
            CheckDirectional();
            isChangingDirection = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D _other) 
    {
        if (_other.CompareTag("StreetBounds"))
        {
            if (IsOnTheStreet)
            {
                IsOnTheStreet = false;
            }
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D _other) 
    {
       if (_other.CompareTag("StreetBounds")) 
       {
           if (!IsOnTheStreet)
           {
               IsOnTheStreet = true;
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
    public RaycastCheckResult CheckForObstacles() 
    {
        RaycastCheckResult result = new RaycastCheckResult();
        Vector3 direction = (GetFollowPathComponent.GetPosition(lastTParameter + 0.1f) - GetFollowPathComponent.GetPosition(lastTParameter)).normalized;
        Vector3 startPosition = transform.position + colliderOffset + (direction * (float)(((colliderRadius) * transform.localScale.x) + 0.1f));
        float distance = (distanceToCheckForStop - colliderRadius) * transform.localScale.x;
        float checkWidth = ((colliderRadius + colliderRadius/4) * 2) * transform.localScale.x;
        Vector3 axis = Vector3.Cross(direction, Vector3.forward);
        GameObject obstacle = PhysicsHelper.RayCastOverALineForFirstGameObject(gameObject, startPosition, axis, checkWidth, direction, distance, layersToCheckCollision, 5);
        if (obstacle) {
            if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car") || obstacle.CompareTag("PlayerCar"))
            {
                result.otherEntity = obstacle.GetComponent<EntityController>();
                result.collided = true;
                // bool onTheStreet = obstacle.GetComponent<EntityController>().IsOnTheStreet;
                // if (onTheStreet)
                // {
                //     stop = true;
                // }
            }
        }
        return result;
    }

    /// <summary>
    /// Checks for a obstacle ahead.true If it's a pedestrian or car stops.
    /// </summary>
    /// <returns>True if there is a obstacle ahead. False if not.</returns>
    public RaycastCheckResult CheckForCollision() 
    {
        RaycastCheckResult result = new RaycastCheckResult();
        Vector3 direction = (GetFollowPathComponent.GetPosition(lastTParameter + 0.1f) - GetFollowPathComponent.GetPosition(lastTParameter)).normalized;
        Vector3 startPosition = transform.position + colliderOffset + (direction * (float)(((colliderRadius) * transform.localScale.x) + 0.1f));
        float distance = (distanceToCheckForCollision - colliderRadius) * transform.localScale.x;
        float checkWidth = ((colliderRadius + colliderRadius/4) * 2) * transform.localScale.x;
        Vector3 axis = Vector3.Cross(direction, Vector3.forward);
        GameObject obstacle = PhysicsHelper.RayCastOverALineForFirstGameObject(gameObject, startPosition, axis, checkWidth, direction, distance, layersToCheckCollision, 5);
        if (obstacle) {
            if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car") || obstacle.CompareTag("PlayerCar"))
            {
                result.otherEntity = obstacle.GetComponent<EntityController>();
                result.collided = true;
                // bool onTheStreet = obstacle.GetComponent<EntityController>().IsOnTheStreet;
                // if (onTheStreet)
                // {
                //     stop = true;
                // }
            }
        }
        return result;
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

    public virtual void OnEntityCollision()
    {
        move = false;
        collisionCheckResult.otherEntity?.OnEntityCollision();
    }

    private void CheckDirectional()
    {
        Vector3 currentDirection = followComponent.GetDirection(lastTParameter) + transform.position;
        Vector3 nextDirection = nextPath.GetDirection(nextPathStarting_t_Parameter) + transform.position;
        Vector3 directional = Vector3.zero;
        float dot = 0.0f;
        // https://math.stackexchange.com/questions/274712/calculate-on-which-side-of-a-straight-line-is-a-given-point-located
        dot = (nextDirection.x - 0.0f)*(currentDirection.y - 0.0f) - (nextDirection.y - 0.0f)*(currentDirection.x - 0.0f);
        if(dot > 0)
        {
            directional = Vector3.right;
        }
        else if(dot < 0)
        {
            directional = -Vector3.right;
        }
        StartDirectional(directional);
    }

    private void StartDirectional(Vector3 _nextDirection)
    {
        onStartChangingDirection?.Invoke(_nextDirection);
    }

    private void StopDirectional()
    {
        onStopChangingDirection?.Invoke();
    }
}
