using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFollowPath : MonoBehaviour
{
    public Action onPathChanged;

    [SerializeField]
    private Path pathToFollow = null;
    [SerializeField]
    private LayerMask directionChangeLayer;

    private bool moveComponent = true;
    private bool isChangingDirection = false;
    private float distanceTravelled = 0.0f;
    private float connected_t_Parameter_ToNextPath;
    private float nextPathStarting_t_Parameter;
    private float lastTParameter = 0.0f;
    private Path nextPath;

    public float PathLength { get; private set; }

    public Vector3 GetCurrentDirection { get => GetDirection(lastTParameter); }

    public Vector3 GetNextPosibleDirection { get => GetDirection(lastTParameter + 0.01f); }

    public Path GetPath { get => pathToFollow; }

    public Path SetPath 
    {
        set 
        {
            pathToFollow = value;
            PathLength = GetLength();
            GetInitialValuesToStartPath();
            onPathChanged?.Invoke();
        }
    }

    void Start() 
    {
        PathLength = GetLength();
        GetInitialValuesToStartPath();
    }

    private void OnEnable()
    {
        GetComponent<EntityDirectionChange>().onStartDirectionChange  += OnStartDirectionChange;
    }

    private void OnDisable()
    {
        GetComponent<EntityDirectionChange>().onStartDirectionChange -= OnStartDirectionChange;
    }

    private void OnDrawGizmos()
    {
        if (pathToFollow)
        {
            float t = pathToFollow.GetTParameter(transform.position);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pathToFollow.GetPointAt(t), transform.position);
        }
    }

    public void GetInitialValuesToStartPath()
    {
        float timeOnCurrentPath = GetTParameter(transform.position);
        distanceTravelled = GetLengthAt(timeOnCurrentPath);
        lastTParameter = timeOnCurrentPath;
    }

    private void OnStartDirectionChange(OnStartDirectionChangeArgs _args)
    {
        nextPath = _args.NextPath;
        nextPathStarting_t_Parameter = nextPath.GetTParameter(transform.position);
        connected_t_Parameter_ToNextPath = GetPath.GetTParameter(nextPath.GetPointAt(nextPathStarting_t_Parameter));
        isChangingDirection = true;
    }

    /// <summary>
    /// Gets the direction at a point t inside the bezier spline.
    /// </summary>
    /// <param name="t">t parameter of the Bezier curve. B(t), must be clamped to 0 and 1. Being 0 the start and 1 the end.</param>
    /// <returns></returns>
    public Vector3 GetDirection(float time) 
    {
        Vector3 directionToReturn = Vector3.zero;
        if (pathToFollow) 
        {
            directionToReturn = pathToFollow.GetDirectionAt(time);
        }
        else
        {
            DebugController.LogErrorMessage("There is no path to follow reference");
        }
        return directionToReturn;
    }

    /// <summary>
    /// Gets the position at t inside the bezier spline.
    /// </summary>
    /// <param name="t">t parameter of the Bezier curve. B(t), must be clamped to 0 and 1. Being 0 the start and 1 the end.</param>
    /// <returns></returns>
    private Vector3 GetPositionAlongPath(float _time)
    {
        Vector3 pointToReturn = Vector3.zero;
        if (isChangingDirection && nextPath)
        {
            if (_time >= connected_t_Parameter_ToNextPath)
            {
                SetPath = nextPath;
                _time = nextPathStarting_t_Parameter;
                isChangingDirection = false;
                nextPath = null;
            }
        }
        pointToReturn = pathToFollow.GetPointAt(_time);
        return pointToReturn;
    }

    public float GetLength() {
        return pathToFollow.GetLength();
    }

    public bool HasPath() {
        bool hasPath = false;
        if (pathToFollow) {
            hasPath = true;
        }
        return hasPath;
    }

    public float GetTParameter(Vector3 _pointToCheck) {
        return pathToFollow.GetTParameter(_pointToCheck);
    }

    public float GetLengthAt(float _tParameter) {
        return pathToFollow.GetLengthAt(_tParameter);
    }

    public bool IsTheEndOfPath(Vector3 _pointToCheck) {
        return !(pathToFollow.GetTParameter(_pointToCheck) < 0.95f);
    }

    public bool IsThereOtherChangeOfDirection()
    {
        Vector3 endOfPath = pathToFollow.GetPointAt(1.0f);
        Vector3 directionVector = endOfPath - transform.position;
        GameObject directionChange = PhysicsHelper.RaycastForFirstGameObject(gameObject, transform.position, directionVector.normalized, directionVector.magnitude, directionChangeLayer, Color.yellow);
        return directionChange != null;
    }

    public Vector3 GetPosition(float _speed, float _deltaTime)
    {
        float _time;
        distanceTravelled += _speed * _deltaTime;
        if (distanceTravelled / PathLength < lastTParameter)
        {
            _time = lastTParameter;
        }
        else
        {
            _time = distanceTravelled / PathLength;
        }
        return GetPositionAlongPath(_time);
    }
}
