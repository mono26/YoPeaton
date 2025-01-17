﻿using System;
using System.Collections;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    public Action onEntityCollision;

    #region Dependencies
    [SerializeField]
    private EntityMovement movementComponent;
    [SerializeField]
    private EntityFollowPath followComponent;
    [SerializeField]
    private EntityAnimationController animationComponent;
    [SerializeField]
    private CollisionCheck collisionComponent;
    [SerializeField]
    private EntityDirectionChange directionChangeComponent;
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
    private bool isThisCarCrossingWithPedestrian;
    #endregion

    private bool move = true;
    private Crosswalk exitedCrosswalk;
    private float colliderRadius;
    private float distanceToCheckForCollision = 0.3f;
    private Vector3 colliderOffset;
    private WaitForSeconds exitedCrosswalkClearWait;

    #region Properties

    public bool IsCrossingCrosswalk { get; private set; }
    public bool IsOnTheStreet { get; private set; }
    public bool IsCrossingIntersection { get; private set; }

    public EntityDirectionChange GetDirectionChangeComponent
    {
        get
        {
            if (directionChangeComponent == null)
            {
                directionChangeComponent = GetComponent<EntityDirectionChange>();
                if (directionChangeComponent == null)
                {
                    DebugController.LogMessage($"Gameobject { gameObject.name } doesn't have EntityDirectionChange component");
                }
            }
            return directionChangeComponent;
        }
    }
    public EntityMovement GetMovableComponent {
        get 
        {
            if (movementComponent == null) {
                movementComponent = GetComponent<EntityMovement>();
                if (movementComponent == null) {
                    DebugController.LogMessage(string.Format("Gameobject {0} doesn't have IMovable component", gameObject.name));
                }
            }
            return movementComponent;
        }
    }

    public EntityFollowPath GetFollowPathComponent {
        get 
        {
            if (followComponent == null) {
                followComponent = GetComponent<EntityFollowPath>();
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

    public EntitySubType GetEntitySubType { get => entitySubType; }

    protected EntitySubType SetEntitySubType { set => entitySubType = value; }

    public EntityType GetEntityType { get => entityType; }
    protected EntityType SetEntityType { set => entityType = value; }
    #endregion

    #region Unity Functions
    //Awake is always called before any Start functions
    protected virtual void Awake()
    {
        var collider = GetComponent<CircleCollider2D>();
        colliderRadius = collider.radius;
        colliderOffset = collider.offset;
        distanceToCheckForCollision = colliderRadius + 0.3f;
        SetRandomEntityType();
        animationComponent = GetComponent<EntityAnimationController>();
        exitedCrosswalkClearWait = new WaitForSeconds(exitedCrosswalkClearTime);
    }

    protected virtual void Start() {
        IsOnTheStreet = false;
        IsCrossingCrosswalk = false;
        if (animationComponent)
        {
            OnEntityMovementEventArgs eventArgs = new OnEntityMovementEventArgs();
            eventArgs.Entity = this;
            eventArgs.MovementDirection = followComponent.GetCurrentDirection;
            animationComponent.OnMovement(eventArgs);
        }
    }

    //protected virtual void Update()
    //{
        
    //}

    protected virtual void UpdateState()
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
            GetMovableComponent?.SlowDownByPercent(0.001f);
        }
    }

    protected virtual void FixedUpdate()
    {
        UpdateState();
        //RaycastCheckResult collisionCheck = HasCollided();
        //if (collisionCheck.collided)
        //{
        //    OnEntityCollision(collisionCheck.otherEntity);
        //}
        //else if (GetFollowPathComponent && GetMovableComponent.GetCurrentSpeed > 0.0f)
        if (GetFollowPathComponent && GetMovableComponent.GetCurrentSpeed > 0.0f)
        {
            GetMovableComponent?.MoveToPosition(GetFollowPathComponent.GetPosition(GetMovableComponent.GetCurrentSpeed, Time.fixedDeltaTime));
        }
    }
    #endregion

    public void SetCrossingWithPedestrianValue(bool value)
    {
        isThisCarCrossingWithPedestrian = value;
    }

    protected abstract void SetRandomEntityType();

    protected abstract bool ShouldStop();

    protected abstract bool ShouldSlowDown();

    protected abstract bool ShouldSpeedUp();

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
            changeDirection = true;
        }
        return changeDirection;
    }

    private void TryChangeDirection(DirectionChange _directionChanger)
    {
        directionChangeComponent.TryChangeDirection(_directionChanger);
    }

    #region Collision check
    /// <summary>
    /// Checks for a obstacle ahead.true If it's a pedestrian or car stops.
    /// </summary>
    /// <returns>True if there is a obstacle ahead. False if not.</returns>
    public RaycastCheckResult CheckForObstacles()
    {
        Vector3 direction = GetFollowPathComponent.GetNextPosibleDirection.normalized;
        Vector3 startPosition = transform.position + (colliderOffset + (direction * ((float)(colliderRadius) + 0.1f))) * transform.localScale.x;
        float distance = (distanceToCheckForStop - colliderRadius) * transform.localScale.x;
        return collisionComponent.CheckForObstacles(direction, startPosition, distance);
    }

    /// <summary>
    /// Checks for a obstacle ahead.true If it's a pedestrian or car stops.
    /// </summary>
    /// <returns>True if there is a obstacle ahead. False if not.</returns>
    public RaycastCheckResult CheckForCollision()
    {
        Vector3 direction = GetFollowPathComponent.GetNextPosibleDirection.normalized;
        Vector3 startPosition = transform.position + (colliderOffset + (direction * ((float)(colliderRadius) + 0.1f))) * transform.localScale.x;
        float distance = (distanceToCheckForCollision - colliderRadius) * transform.localScale.x;
        float checkWidth = (colliderRadius * 2) * transform.localScale.x;
        Vector3 axis = Vector3.Cross(direction, Vector3.forward);
        return collisionComponent.CheckForCollision(direction, startPosition, axis, distance, checkWidth); ;
    }

    /// <summary>
    /// Checks using a raycast if there is a obstacle in front of the entity.
    /// </summary>
    /// <returns></returns>
    public RaycastCheckResult HasAObstacleUpFront()
    {
        RaycastCheckResult obstacleCheckResult = default;
        if (IsOnTheStreet /* && !aiEntity.IsCrossingCrosswalk */)
        {
            obstacleCheckResult = CheckForObstacles();
        }
        return obstacleCheckResult;
    }

    public RaycastCheckResult HasCollided()
    {
        RaycastCheckResult collisionCheckResult = default;
        if (IsOnTheStreet && GetMovableComponent.GetCurrentSpeed != 0)
        {
            collisionCheckResult = CheckForCollision();
            if (collisionCheckResult.collided && collisionCheckResult.otherEntity.IsOnTheStreet)
            {
                DebugController.LogMessage($"Llamar metodo para cambiar a animacion de atropellado, { gameObject.name } Atropello a: { collisionCheckResult.otherEntity.name }");
            }
        }
        return collisionCheckResult;
    }

    protected virtual void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("ChangeOfDirection"))
        {
            if (ShouldChangeDirection())
            {
                DirectionChange directionChanger = _other.GetComponent<DirectionChange>();
                //Debug.LogError("Nombre: " + this.gameObject.name + ", Changed Direction.");
                TryChangeDirection(directionChanger);
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
    #endregion

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

    public virtual void OnEntityCollision(EntityController _otherEntity)
    {
        move = false;
        onEntityCollision?.Invoke();
        if (_otherEntity)
        {
            _otherEntity?.OnEntityCollision(null);
        }
    }

    public virtual void OnStartedCrossing(ICrossable _crossable)
    {
        switch (_crossable.CrossableType)
        {
            case CrossableType.CrossWalk:
                {
                    OnStartCrossingCrossWalk(_crossable);
                    break;
                }
            case CrossableType.Intersection:
                {
                    OnStartCrossingIntersection(_crossable);
                    break;
                }
        }
    }

    public void OnFinishedCrossing(ICrossable _crossable)
    {
        switch (_crossable.CrossableType)
        {
            case CrossableType.CrossWalk:
                {
                    OnFinishCrossingCrossWalk(_crossable);
                    break;
                }
            case CrossableType.Intersection:
                {
                    OnFinishCrossingIntersection(_crossable);
                    break;
                }
        }
    }

    #region On start methods
    protected virtual void OnStartCrossingCrossWalk(ICrossable _crossWalk)
    {
        IsCrossingCrosswalk = true;
    }

    protected virtual void OnStartCrossingIntersection(ICrossable _intersection)
    {
        IsCrossingIntersection = true;
    }
    #endregion

    #region On finish methods
    protected virtual void OnFinishCrossingCrossWalk(ICrossable _crossWalk)
    {
        IsCrossingCrosswalk = false;
    }

    protected virtual void OnFinishCrossingIntersection(ICrossable _intersection)
    {
        IsCrossingIntersection = false;
    }
    #endregion

    public void OnCrossableEntered(ICrossable _crossable)
    {
        switch (_crossable.CrossableType)
        {
            case CrossableType.CrossWalk:
                {
                    OnCrossWalkEntered(_crossable);
                    break;
                }
            case CrossableType.Intersection:
                {
                    OnIntersectionEntered(_crossable);
                    break;
                }
        }
    }

    public void OnCrossableExited(ICrossable _crossable)
    {
        switch (_crossable.CrossableType)
        {
            case CrossableType.CrossWalk:
                {
                    OnCrossWalkExited(_crossable);
                    OnFinishedCrossing(_crossable);
                    break;
                }
            case CrossableType.Intersection:
                {
                    OnIntersectionExited(_crossable);
                    OnFinishedCrossing(_crossable);
                    break;
                }
        }
    }

    public abstract void OnCrossWalkEntered(ICrossable _crossWalk);

    public virtual void OnCrossWalkExited(ICrossable _crossWalk)
    {
        exitedCrosswalk = _crossWalk as Crosswalk;
        StartCoroutine(ClearExitedCrossWalk());
    }

    public abstract void OnIntersectionEntered(ICrossable _crossWalk);

    public abstract void OnIntersectionExited(ICrossable _intersection);
}
