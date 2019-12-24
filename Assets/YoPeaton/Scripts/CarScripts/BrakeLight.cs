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

    private void Awake()
    {
        EntityMovement component = GetComponent<EntityMovement>();
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
        if (_type.Equals(VehicleType.Car))
        {
            carLights.SetActive(_activate);
            motorcycleLights.SetActive(false);
        }
        else
        {
            carLights.SetActive(false);
            motorcycleLights.SetActive(_activate);
        }
    }
}
