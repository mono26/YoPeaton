using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TurnCooldownInfo
{
    public EntityType Type { get; set; }
    public System.DateTime CooldownFinish { get; set; }

    public const float DEFAULT_COOLDOWN = 9.0f;
    
    public bool IsInCooldown()
    {
        return CooldownFinish > System.DateTime.UtcNow;
    }
}
