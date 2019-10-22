using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaitTicket
{
    public EntityController waitingEntity;
    public System.DateTime waitStartTime;

    public WaitTicket(EntityController _entity) {
        waitingEntity = _entity;
        waitStartTime = System.DateTime.UtcNow;
    }
}
