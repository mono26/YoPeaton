﻿using System;
using System.Collections;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    public Action<OnEntityStartDirectionChangeArgs> onStartDirectionChange;
    public Action onStopChangingDirection;
    public Action<Vector3> onStartDirectional;
    public Action onEntityCollision;
    public Action<OnEntityMovementEventArgs> onDirectionChange;

    #region Dependencies
    [SerializeField]
    private EntityMovement movementComponent;
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
    private Direction currentDirection;
    [SerializeField]
    private LayerMask layersToCheckCollision;
    [SerializeField]
    private bool isThisCarCrossingWithPedestrian;
    #endregion

    private bool canTurn = true;
    private Crosswalk exitedCrosswalk;
    private float colliderRadius;
    private float distanceTravelled = 0.0f;
    private float distanceToCheckForCollision = 0.1f;
    private float lastTParameter = 0.0f;
    public bool IsCrossingCrosswalk { get; private set; }
    public bool IsOnTheStreet { get; private set; }
    private bool move = true;
    protected bool entityIsPlayer = true;
    protected RaycastCheckResult collisionCheckResult;
    private Vector3 colliderOffset;
    private WaitForSeconds exitedCrosswalkClearWait;

    #region Properties

    public EntityMovement GetMovableComponent {
        get {
            if (movementComponent == null) {
                movementComponent = GetComponent<EntityMovement>();
                if (movementComponent == null) {
                    DebugController.LogMessage(string.Format("Gameobject {0} doesn't have IMovable component", gameObject.name));
                }
            }
            return movementComponent;
        }
        set
        {
            GetMovableComponent = value;
        }
    }

    public FollowPath GetFollowPathComponent {
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

    public Path GetCurrentPath
    {
        get
        {
            return followComponent.GetPath;
        }
    }

    public bool GetisThisCarCrossingWithPedestrian
    {
        get
        {
            return isThisCarCrossingWithPedestrian;
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

#region Unity Functions
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
        IsCrossingCrosswalk = false;
        GetInitialValuesToStartPath();
        if (animationComponent) {
            OnEntityMovementEventArgs eventArgs = new OnEntityMovementEventArgs();
            eventArgs.Entity = this;
            eventArgs.MovementDirection = followComponent.GetDirection(lastTParameter);
            animationComponent.OnMovement(eventArgs);
        }
    }

    private void OnEnable()
    {
        if (followComponent)
        {
            followComponent.onPathChanged += OnPathChanged;
        }
    }

    private void OnDisable()
    {
        if (followComponent)
        {
            followComponent.onPathChanged -= OnPathChanged;
        }
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
        else if (ShouldSpeedUp()) 
        {
            GetMovableComponent?.SpeedUp(deltaTime);
        }
        else
        {
            // Slow down by friction.
            GetMovableComponent?.SlowDownByPercent(3f);
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
                if (distanceTravelled / GetFollowPathComponent.PathLength < lastTParameter) 
                {
                    t = lastTParameter;
                }
                else 
                {
                    t = distanceTravelled / GetFollowPathComponent.PathLength;
                }
                if (!t.Equals(lastTParameter)) 
                {
                    GetMovableComponent?.MoveToPosition(GetFollowPathComponent.GetPosition(t));
                    lastTParameter = t;
                }
            }
        }
    }
#endregion

    public void SetCrossingWithPedestrianValue(bool value)
    {
        isThisCarCrossingWithPedestrian = value;
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
            if(probability < 0.25f)
            {
                entitySubType = EntitySubType.Motorcycle;
            } else if (probability < 0.60f)
            {
                entitySubType = EntitySubType.GreenCar;
            }
            else if (probability < 0.80f)
            {
                entitySubType = EntitySubType.RedCar;
            } else
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

    private void OnPathChanged()
    {
        GetInitialValuesToStartPath();
        if (entityType.Equals(EntityType.Car))
        {
            movementComponent.SlowDownByPercent(50.0f);
            StopDirectional();
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

    protected abstract bool ShouldSpeedUp();

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

    public Path nextPathReference;

    private void TryChangeDirection(DirectionChange _directionChanger)
    {
        Path nextPath = _directionChanger.GetConnectionFrom(followComponent.GetPath);
        nextPathReference = nextPath;
        if (nextPath) 
        {
            Vector3 currentDirection = followComponent.GetDirection(lastTParameter);
            Vector3 nextDirection = nextPath.GetDirectionAt(nextPath.GetTParameter(transform.position));
            OnEntityStartDirectionChangeArgs eventArgs = new OnEntityStartDirectionChangeArgs();
            OnEntityMovementEventArgs movementArgs = new OnEntityMovementEventArgs();
            eventArgs.Entity = this;
            eventArgs.Direction = nextDirection;
            eventArgs.NextPath = nextPath;
            movementArgs.MovementDirection = nextDirection;
            movementArgs.Entity = this;
            onStartDirectionChange?.Invoke(eventArgs);
            //onDirectionChange?.Invoke(movementArgs);
            CheckDirectional(currentDirection, nextDirection);
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
        Vector3 startPosition = transform.position + (colliderOffset + (direction * ((float)(colliderRadius) + 0.1f))) * transform.localScale.x;
        float distance = (distanceToCheckForStop - colliderRadius) * transform.localScale.x;
        float checkWidth = ((colliderRadius + colliderRadius/4) * 2) * transform.localScale.x;
        Vector3 axis = Vector3.Cross(direction, Vector3.forward);
        // GameObject obstacle = PhysicsHelper.RaycastOverALineForFirstGameObject(gameObject, startPosition, axis, checkWidth, direction, distance, layersToCheckCollision, 5);
        GameObject obstacle = PhysicsHelper.RaycastInAConeForFirstGameObject(gameObject, startPosition, direction, distance, layersToCheckCollision, 90.0f, 5);
        if (obstacle) {
            if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car") || obstacle.CompareTag("PlayerCar"))
            {
                result.otherEntity = obstacle.GetComponent<EntityController>();
                result.collided = true;
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
        Vector3 startPosition = transform.position + (colliderOffset + (direction * ((float)(colliderRadius) + 0.1f))) * transform.localScale.x;
        float distance = (distanceToCheckForCollision - colliderRadius) * transform.localScale.x;
        float checkWidth = (colliderRadius * 2) * transform.localScale.x;
        Vector3 axis = Vector3.Cross(direction, Vector3.forward);
        GameObject obstacle = PhysicsHelper.RaycastOverALineForFirstGameObject(gameObject, startPosition, axis, checkWidth, direction, distance, layersToCheckCollision, 5);
        if (obstacle) {
            if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car"))
            {
                result.otherEntity = obstacle.GetComponent<EntityController>();
                result.collided = true;
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

    // TODO only cars!!!

    private void CheckDirectional(Vector3 _currentDirection, Vector3 _nextDirection)
    {
        Vector3 directional = Vector3.zero;
        float dot = 0.0f;
        // https://math.stackexchange.com/questions/274712/calculate-on-which-side-of-a-straight-line-is-a-given-point-located
        dot = (_nextDirection.x - 0.0f)*(_currentDirection.y - 0.0f) - (_nextDirection.y - 0.0f)*(_currentDirection.x - 0.0f);
        if(dot > 0)
        {
            directional = Vector3.right;
        }
        else if(dot < 0)
        {
            directional = -Vector3.right;
        }
        onStartDirectional?.Invoke(_nextDirection);
    }

    private void StopDirectional()
    {
        onStopChangingDirection?.Invoke();
    }

    public virtual void OnStartedCrossing()
    {
        IsCrossingCrosswalk = true;
    }

    public virtual void OnFinishedCrossing()
    {
        IsCrossingCrosswalk = false;
    }
}
