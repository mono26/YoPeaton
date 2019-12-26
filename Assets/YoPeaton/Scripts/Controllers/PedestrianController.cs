using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : AIController
{
    protected override void Start()
    {
        base.Start();
        SetEntityType = EntityType.Pedestrian;
    }

    protected override void SetRandomEntityType()
    {
        float probability = UnityEngine.Random.Range(0f, 1f);
        if (probability < 0.3f)
        {
            SetEntitySubType = EntitySubType.Male;
        }
        else if (probability < 0.6f)
        {
            SetEntitySubType = EntitySubType.Female;
        }
        else if (probability < 0.67f)
        {
            SetEntitySubType = EntitySubType.MaleWithBaby;
        }
        else if (probability < 0.74f)
        {
            SetEntitySubType = EntitySubType.FemaleWithBaby;
        }
        else if (probability < 0.84f)
        {
            SetEntitySubType = EntitySubType.MaleWithDog;
        }
        else if (probability < 0.94f)
        {
            SetEntitySubType = EntitySubType.FemaleWithDog;
        }
        else if (probability < 0.97f)
        {
            SetEntitySubType = EntitySubType.MaleWithWalker;
        }
        else if (probability < 1f)
        {
            SetEntitySubType = EntitySubType.FemaleWithWalker;
        }
    }
}
