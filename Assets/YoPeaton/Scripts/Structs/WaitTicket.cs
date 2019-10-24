using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaitTicket
{
    public EntityController waitingEntity;
    public System.DateTime waitStartTime;
    public static readonly WaitTicket invalidTicket = new WaitTicket() { waitingEntity = null, waitStartTime = System.DateTime.MinValue};

    public WaitTicket(EntityController _entity) {
        waitingEntity = _entity;
        waitStartTime = System.DateTime.UtcNow;
    }
}
