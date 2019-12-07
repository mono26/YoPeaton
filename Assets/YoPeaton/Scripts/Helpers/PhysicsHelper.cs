using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsHelper
{
    public static void CircleCastForFirstGameObject(GameObject _castingGO, Vector3 _startPosition, float _checkRadius, Vector3 _direction, float _distance, LayerMask _layersToCheckCollision) {
        GameObject goToReturn = null;
        GameObject objectHit = null;
        RaycastHit2D castHit = Physics2D.CircleCast((Vector2)_startPosition, _checkRadius, _direction, _distance, _layersToCheckCollision);
        if (castHit.collider) {
            objectHit = castHit.collider.gameObject;
            if (objectHit && !objectHit.Equals(_castingGO)) {
                DebugController.DrawDebugLine(_startPosition, castHit.point, Color.magenta);
                goToReturn = objectHit;
                DebugController.DrawDebugCircle(_startPosition, _checkRadius, Color.green);
                DebugController.DrawDebugRay(_startPosition, _direction, _distance, Color.green);
                DebugController.DrawDebugCircle(_startPosition + _direction * _distance, _checkRadius, Color.green);
            }
        }
    }

    public static void BoxCastForFirstGameObject(GameObject _castingGO, Vector3 _startPosition, Vector3 _size, float _angle, Vector3 _direction, float _distance, LayerMask _layersToCheckCollision) {
        GameObject goToReturn = null;
        GameObject objectHit = null;
        RaycastHit2D castHit = Physics2D.BoxCast((Vector2)_startPosition, _size, _angle, _direction, _distance, _layersToCheckCollision);
        if (castHit.collider) {
            objectHit = castHit.collider.gameObject;
            if (objectHit && !objectHit.Equals(_castingGO)) {
                if (objectHit.CompareTag("Pedestrian") || objectHit.CompareTag("Car")) {
                    // DebugController.DrawDebugLine(_startPosition, castHit.point, Color.magenta);
                    goToReturn = objectHit;
                    // DebugController.DrawDebugCircle(_startPosition, _checkRadius, Color.green);
                    DebugController.DrawDebugRay(_startPosition, _direction, _distance, Color.green);
                    // DebugController.DrawDebugCircle(_startPosition + _direction * _distance, _checkRadius, Color.green);
                }
            }
        }
    }

    /// <summary>
    /// Casts a number of raycasts a long a line of certain width and defined by an axis towards specified layers.
    /// </summary>
    /// <param name="_castingGO">GameObject that cast the ray.</param>
    /// <param name="_startPosition">Position of reference for the axis and raycasts.</param>
    /// <param name="_checkAxis">Axis that defines the axis.</param>
    /// <param name="_lineLenght">Lenght of the line.</param>
    /// <param name="_direction">Direction of the rays.</param>
    /// <param name="_distance">Distance for the rays to travel.</param>
    /// <param name="_layersToCheckCollision">Layers to check collision with.</param>
    /// <param name="_numberOfRaycast">Number of rays to cast.</param>
    /// <returns></returns>
    public static GameObject RaycastOverALineForFirstGameObject(GameObject _castingGO, Vector3 _startPosition, Vector3 _checkAxis, float _lineLenght, Vector3 _direction, float _distance, LayerMask _layersToCheckCollision, int _numberOfRaycast) {
        GameObject goToReturn = null;
        float checkIncrement =_lineLenght / (float)_numberOfRaycast;
        Vector3 startPosition;
        for (int i = 0; i < _numberOfRaycast; i++) {
            startPosition = _startPosition + (_checkAxis * ((_lineLenght / 2)
             - (checkIncrement * i)));
            goToReturn = RaycastForFirstGameObject(_castingGO, startPosition, _direction, _distance, _layersToCheckCollision);
            if (goToReturn) 
            {
                break;
            }
        }
        return goToReturn;
    }

    public static GameObject RaycastForFirstGameObject(GameObject _castingGO, Vector3 _startPosition, Vector3 _direction, float _distance, LayerMask _layersToCheckCollision) 
    {
        GameObject goToReturn = null;
        GameObject objectHit = null;
        RaycastHit2D castHit = Physics2D.Raycast(_startPosition, _direction, _distance, _layersToCheckCollision);
        DebugController.DrawDebugRay(_startPosition, _direction, _distance, Color.magenta);
        DebugController.DrawDebugRay(_startPosition, _direction, _distance, Color.magenta);
        if (castHit.collider) 
        {
            objectHit = castHit.collider.gameObject;
            if (objectHit && !objectHit.Equals(_castingGO)) 
            {
                DebugController.DrawDebugLine(_startPosition, castHit.point, Color.cyan);
                goToReturn = objectHit;
            }
        }
        return goToReturn;
    }

    public static GameObject RaycastInAConeForFirstGameObject(GameObject _castingGO, Vector3 _startPosition, Vector3 _direction, float _distance, LayerMask _layersToCheckCollision, float _coneAngle, int _numberOfRaycast)
    {
        GameObject goToReturn = null;
        float checkIncrement = _coneAngle / (float)_numberOfRaycast;
        Vector3 direction;
        float angle;
        for (int i = 0; i < _numberOfRaycast; i++)
        {
            angle = (-_coneAngle / 2) + (i * checkIncrement);
            direction = Quaternion.AngleAxis(angle, Vector3.forward) * _direction;
            goToReturn = RaycastForFirstGameObject(_castingGO, _startPosition, direction, _distance, _layersToCheckCollision);
            if (goToReturn)
            {
                break;
            }
        }
        return goToReturn;
    }
}
