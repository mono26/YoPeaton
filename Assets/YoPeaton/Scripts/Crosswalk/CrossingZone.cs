using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingZone : MonoBehaviour
{
    [SerializeField]
    private Crosswalk crosswalkReference;

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.CompareTag("Car") || _other.CompareTag("Pedestrian"))
        {
            EntityController entity = _other.transform.GetComponent<EntityController>();
            crosswalkReference.OnExitedCrossingZone(entity);
        }
    }
}
