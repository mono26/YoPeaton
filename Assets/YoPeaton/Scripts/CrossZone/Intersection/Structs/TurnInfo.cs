﻿using System;

public struct TurnInfo
{
    public EntityType Type { get; set; }
    public DateTime EndTime { get; set; }

    public const float DEFAULT_DURATION = 9.0f;
}
