using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingZone : MonoBehaviour
{
    [SerializeField]
    private Crosswalk crosswalkReference;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.CompareTag("Pedestrian") || _other.gameObject.CompareTag("Car"))
        {
            EntityController entity = _other.transform.GetComponent<EntityController>();
            crosswalkReference.OnEnteredWaitingZone(entity);
        }
    }
}
