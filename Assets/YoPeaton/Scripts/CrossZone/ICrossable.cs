using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrossable
{
    CrossableType CrossableType { get; set; }
    bool CanCross(EntityController _entity);
    void OnStartedCrossing(EntityController _entity);
    void OnFinishedCrossing(EntityController _entity);
    void OnEnter(EntityController _entity);
    void OnExited(EntityController _entity);
}
