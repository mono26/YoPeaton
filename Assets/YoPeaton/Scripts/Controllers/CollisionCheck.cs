using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    [SerializeField]
    private LayerMask layersToCheckCollision;

    /// <summary>
    /// Checks for a obstacle ahead.true If it's a pedestrian or car stops.
    /// </summary>
    /// <returns>True if there is a obstacle ahead. False if not.</returns>
    public RaycastCheckResult CheckForObstacles(Vector3 _direction, Vector3 _startPosition, float _distance)
    {
        RaycastCheckResult result = new RaycastCheckResult();
        GameObject obstacle = PhysicsHelper.RaycastInAConeForFirstGameObject(gameObject, _startPosition, _direction, _distance, layersToCheckCollision, 70.0f, 5);
        if (obstacle)
        {
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
    public RaycastCheckResult CheckForCollision(Vector3 _direction, Vector3 _startPosition, Vector3 _axis, float _distance, float _checkWidth)
    {
        RaycastCheckResult result = new RaycastCheckResult();
        GameObject obstacle = PhysicsHelper.RaycastOverALineForFirstGameObject(gameObject, _startPosition, _axis, _checkWidth, _direction, _distance, layersToCheckCollision, 5);
        if (obstacle)
        {
            if (obstacle.CompareTag("Pedestrian") || obstacle.CompareTag("Car"))
            {
                result.otherEntity = obstacle.GetComponent<EntityController>();
                result.collided = true;
            }
        }
        return result;
    }
}
