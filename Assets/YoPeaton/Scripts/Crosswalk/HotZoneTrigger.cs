using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private Crosswalk owner;

    public Crosswalk GetOwner {
        get {
            return owner;
        }
    }

    private void OnTriggerEnter2D(Collider2D _other) {
        if (_other.CompareTag("Pedestrian") || _other.CompareTag("Car")) {
            owner?.OnEntering(_other.GetComponent<EntityController>());
        }
    }

    private void OnTriggerExit2D(Collider2D _other) {
        if (_other.CompareTag("Pedestrian") || _other.CompareTag("Car")) {
            owner?.OnExited(_other.GetComponent<EntityController>());
        }
    }
}
