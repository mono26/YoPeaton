using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : AIController
{
    protected override void Start()
    {
        if (GetDirectionChangeComponent)
        {
            GetDirectionChangeComponent.onStartDirectionChange += OnStartDirectionChange;
            GetDirectionChangeComponent.onStopChangingDirection += OnStopChangingDirection;
        }
    }

    private void OnDestroy()
    {
        if (GetDirectionChangeComponent)
        {
            GetDirectionChangeComponent.onStopChangingDirection -= OnStopChangingDirection;
            GetDirectionChangeComponent.onStopChangingDirection -= OnStopChangingDirection;
        }
    }

    protected override void SetRandomEntityType()
    {
        float probability = UnityEngine.Random.Range(0f, 1f);
        BrakeLight brakeLights = GetComponent<BrakeLight>();
        if (probability < 0.25f)
        {
            SetEntitySubType = EntitySubType.Motorcycle;
            brakeLights.VehicleType = VehicleType.Motorcycle;
        }
        else if (probability < 0.60f)
        {
            SetEntitySubType = EntitySubType.GreenCar;
            brakeLights.VehicleType = VehicleType.Car;
        }
        else if (probability < 0.8f)
        {
            SetEntitySubType = EntitySubType.RedCar;
            brakeLights.VehicleType = VehicleType.Car;
        }
        else if (probability <= 1f)
        {
            SetEntitySubType = EntitySubType.YellowCar;
            brakeLights.VehicleType = VehicleType.Car;
        }
    }

    private void OnStartDirectionChange(OnStartDirectionChangeArgs _eventArgs)
    {
        CheckDirectional(_eventArgs.CurrentDirection, _eventArgs.NextDirection);
    }

    // TODO only cars!!!
    private void CheckDirectional(Vector3 _currentDirection, Vector3 _nextDirection)
    {
        Vector3 directional = Vector3.zero;
        float dot = 0.0f;
        // https://math.stackexchange.com/questions/274712/calculate-on-which-side-of-a-straight-line-is-a-given-point-located
        dot = (_nextDirection.x - 0.0f) * (_currentDirection.y - 0.0f) - (_nextDirection.y - 0.0f) * (_currentDirection.x - 0.0f);
        if (dot > 0)
        {
            directional = Vector3.right;
        }
        else if (dot < 0)
        {
            directional = -Vector3.right;
        }
        //onStartDirectional?.Invoke(_nextDirection);
    }

    private void OnStopChangingDirection()
    {
        StopDirectional();
    }

    private void StopDirectional()
    {
        // onStopChangingDirection?.Invoke();
    }
}
