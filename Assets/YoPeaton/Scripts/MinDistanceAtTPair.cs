using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MinDistanceAtTPair
{
    public float tParameter;
    public float minSqrDistance;

    public MinDistanceAtTPair (float _tParameter = float.NaN, float _minDistance = float.NaN)
    {
        tParameter = _tParameter;
        minSqrDistance = _minDistance;
    }
}
