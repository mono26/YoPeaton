using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour, ICrossable
{
    [SerializeField]
    private PathFreePassInfo[] pathsInfo;
    private readonly List<EntityController> crossingCars = new List<EntityController>();

    public CrossableType CrossableType { get; private set; }

    private void Start()
    {
        CrossableType = CrossableType.Intersection;
    }

    public bool CanCross(EntityController _entity)
    {
        bool canCross = true;
        if ((crossingCars != null && crossingCars.Count > 0) && (pathsInfo != null && pathsInfo.Length > 0))
        {
            for (int i = 0; i < pathsInfo.Length; i++)
            {
                if (pathsInfo[i].path == _entity.GetCurrentPath)
                {
                    for (int j = 0; j < pathsInfo[i].pathsToCheck.Length; j++)
                    {
                        for (int k = 0; k < crossingCars.Count; k++)
                        {
                            if (crossingCars[k] != _entity)
                            {
                                if (pathsInfo[i].pathsToCheck[j] == crossingCars[k].GetCurrentPath)
                                {
                                    canCross = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        return canCross;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.CompareTag("Car"))
        {
            EntityController entity = _other.GetComponent<EntityController>();
            OnEnter(entity); 
            entity.OnCrossableEntered(this);
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.gameObject.CompareTag("Car"))
        {
            EntityController entity = _other.GetComponent<EntityController>();
            OnExited(entity);
            entity.OnCrossableExited(this);
        }
    }

    public void OnStartedCrossing(EntityController _entity)
    {
        if (!crossingCars.Contains(_entity))
        {
            crossingCars.Add(_entity);
        }
    }

    public void OnFinishedCrossing(EntityController _entity)
    {
        if (crossingCars.Contains(_entity))
        {
            crossingCars.Remove(_entity);
        }
    }

    public void OnEnter(EntityController _entity)
    {
        OnStartedCrossing(_entity);
    }

    public void OnExited(EntityController _entity)
    {
        OnFinishedCrossing(_entity);
    }
}
