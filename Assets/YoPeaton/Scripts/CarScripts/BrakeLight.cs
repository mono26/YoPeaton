using UnityEngine;

public class BrakeLight : MonoBehaviour
{
    [SerializeField]
    private GameObject motorcycleLights;
    [SerializeField]
    private GameObject carLights;

    [SerializeField]
    private VehicleType vehicleType;
    public VehicleType VehicleType { get => vehicleType; set => vehicleType = value; }

    private EntityMovement component;

    private void Awake()
    {
        component = GetComponent<EntityMovement>();
        component.AddOnAccelerate(OnAccelerate);
        component.AddOnBrake(OnBrake);
    }

    private void OnAccelerate(EntityController _entity)
    {
        ActivateBrake(VehicleType, false);
    }

    private void OnBrake(EntityController _entity)
    {
        ActivateBrake(VehicleType, true);
    }

    public void ActivateBrake(VehicleType _type, bool _activate)
    {
        if(component.direction == new Vector3(0, 1, 0))
        {
            if (_type.Equals(VehicleType.Car))
            {
                if (carLights)
                {
                    carLights.SetActive(_activate);
                }
                if (motorcycleLights)
                {
                    motorcycleLights.SetActive(false);
                }
            }
            else
            {
                if (carLights)
                {
                    carLights.SetActive(false);
                }
                if (motorcycleLights)
                {
                    motorcycleLights.SetActive(_activate);
                }
            }
        }
        
    }
}
