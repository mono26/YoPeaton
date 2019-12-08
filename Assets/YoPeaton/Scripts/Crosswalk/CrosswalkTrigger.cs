using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswalkTrigger : MonoBehaviour
{
    [SerializeField]
    private Crosswalk crosswalk;

    public Crosswalk GetCrosswalk { get { return crosswalk;} }
}
