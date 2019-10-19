using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosswalk : MonoBehaviour
{
    [SerializeField]
    private List<EntityController> waitingPedestrians;
    [SerializeField]
    private List<EntityController> waitingCars;
    [SerializeField]
    private List<EntityController> crossingPedestrians;
    [SerializeField]
    private List<EntityController> crossingCars;
    [SerializeField]
    private List<EntityController> finishingCross;

    public void OnEntering(EntityController _entity) {
        if (_entity.CompareTag("Pedestrian")) {
            if (!waitingPedestrians.Contains(_entity) && !finishingCross.Contains(_entity)) {
                waitingPedestrians.Add(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) {
            if (!waitingCars.Contains(_entity) && !finishingCross.Contains(_entity)) {
                waitingCars.Add(_entity);
            }
        }
    }

    public void OnExited(EntityController _entity) {
        if (_entity.CompareTag("Pedestrian")) {
            if (waitingPedestrians.Contains(_entity)) {
                waitingPedestrians.Remove(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) {
            if (waitingCars.Contains(_entity)) {
                waitingCars.Remove(_entity);
            }
        }
        if (finishingCross.Contains(_entity)) {
            finishingCross.Remove(_entity);
        }
    }

    public void OnStartedCrossing(EntityController _entity) {
        if (_entity.CompareTag("Pedestrian")) {
            if (!crossingPedestrians.Contains(_entity)) {
                crossingPedestrians.Add(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) {
            if (!crossingCars.Contains(_entity)) {
                crossingCars.Add(_entity);
            }
        }
    }

    public void OnFinishedCrossing(EntityController _entity) {
        if (_entity.CompareTag("Pedestrian"))
        {
            if (crossingPedestrians.Contains(_entity)) {
                crossingPedestrians.Remove(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) {
            if (crossingCars.Contains(_entity)) {
                crossingCars.Remove(_entity);
            }
        }
        finishingCross.Add(_entity);
    }

    public bool CanCross(string _entityType)
    {
        bool cross = true;
        if (_entityType.Equals("Car")) {
            if (crossingPedestrians.Count > 0 || waitingPedestrians.Count > 0) {
                cross = false;
            }
        }
        else if (_entityType.Equals("Pedestrian")) {
            if (crossingCars.Count > 0) {
                cross = false;
            }
        }
        return cross;
    }

    public bool JustFinishedCrossing(EntityController _entity) {
        bool justCrossed = false;
        if (finishingCross.Contains(_entity)) {
            justCrossed = true;
        }
        return justCrossed;
    }
}
