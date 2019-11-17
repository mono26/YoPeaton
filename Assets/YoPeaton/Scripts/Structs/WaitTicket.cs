using System;

[System.Serializable]
public struct WaitTicket
{
    public System.DateTime waitStartTime;
    public bool gaveCross;
    public System.DateTime gaveCrossTime;
    public static readonly WaitTicket invalidTicket = new WaitTicket() { waitStartTime = System.DateTime.MinValue, gaveCrossTime = System.DateTime.MinValue };

    public const float maxWaitTimeInSeconds = 30.0f;
}
