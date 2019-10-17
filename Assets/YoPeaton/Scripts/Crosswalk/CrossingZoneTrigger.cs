using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private string hotZoneType = "";
    [SerializeField]
    private Crosswalk owner;

    private void OnTriggerEnter2D(Collider2D _other) {
        if (_other.CompareTag("Pedestrian") || _other.CompareTag("Car")) {
            owner?.OnStartedCrossing(_other.GetComponent<EntityController>());
        }
    }

    private void OnTriggerExit2D(Collider2D _other) {
        if (_other.CompareTag("Pedestrian") || _other.CompareTag("Car")) {
            owner?.OnFinishedCrossing(_other.GetComponent<EntityController>());
        }
    }
}
