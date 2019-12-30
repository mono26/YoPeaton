using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private Crosswalk crosswalkReference;

    private void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.CompareTag("Pedestrian") || _other.CompareTag("Car")) 
        {
            crosswalkReference?.OnStartedCrossing(_other.GetComponent<EntityController>());
        }
    }

    private void OnTriggerExit2D(Collider2D _other) 
    {
        if (_other.CompareTag("Pedestrian") || _other.CompareTag("Car")) 
        {
            crosswalkReference?.OnFinishedCrossing(_other.GetComponent<EntityController>());
        }
    }
}
