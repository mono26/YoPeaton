using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaitTicket
{
    public EntityController waitingEntity;
    public System.DateTime waitStartTime;
    public bool gaveCross;
    public System.DateTime gaveCrossTime;
    public static readonly WaitTicket invalidTicket = new WaitTicket() { waitingEntity = null, waitStartTime = System.DateTime.MinValue, gaveCrossTime = System.DateTime.MinValue };

    public const float maxWaitTimeInMinutes = 3.0f;

    public WaitTicket(EntityController _entity) {
        waitingEntity = _entity;
        waitStartTime = System.DateTime.UtcNow;
        gaveCross = false;
        gaveCrossTime = waitStartTime.AddMinutes(maxWaitTimeInMinutes);
    }
}
