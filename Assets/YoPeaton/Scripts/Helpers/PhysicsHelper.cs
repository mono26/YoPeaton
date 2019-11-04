using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsHelper
{
    private static GameObject objectHit;
    private static GameObject goToReturn;
    private static RaycastHit2D castHit;

    public static void CircleCastForFirstGameObject(GameObject _castingGO, Vector3 _startPosition, float _checkRadius, Vector3 _direction, float _distance, LayerMask _layersToCheckCollision) {
        goToReturn = null;
        objectHit = null;
        castHit = Physics2D.CircleCast((Vector2)_startPosition, _checkRadius, _direction, _distance, _layersToCheckCollision);
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
        goToReturn = null;
        objectHit = null;
        castHit = Physics2D.BoxCast((Vector2)_startPosition, _size, _angle, _direction, _distance, _layersToCheckCollision);
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
    /// <param name="_numberOfRayCasts">Number of rays to cast.</param>
    /// <returns></returns>
    public static GameObject RayCastOverALineForFirstGameObject(GameObject _castingGO, Vector3 _startPosition, Vector3 _checkAxis, float _lineLenght, Vector3 _direction, float _distance, LayerMask _layersToCheckCollision, int _numberOfRayCasts) {
        goToReturn = null;
        objectHit = null;
        float checkIncrement =_lineLenght / (float)_numberOfRayCasts;
        Vector3 startPosition;
        for (int i = 0; i < _numberOfRayCasts; i++) {
            startPosition = _startPosition + (_checkAxis * ((_lineLenght / 2)
             - (checkIncrement * i)));
            castHit = Physics2D.Raycast(startPosition, _direction, _distance, _layersToCheckCollision);
            DebugController.DrawDebugRay(startPosition, _direction, _distance, Color.magenta);
            DebugController.DrawDebugRay(startPosition, _direction, _distance, Color.magenta);
            if (castHit.collider) {
                objectHit = castHit.collider.gameObject;
                if (objectHit && !objectHit.Equals(_castingGO)) {
                    DebugController.DrawDebugLine(_startPosition, castHit.point, Color.magenta);
                    goToReturn = objectHit;
                    break;
                }
            }
        }
        return goToReturn;
    }
}
