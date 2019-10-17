using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    protected override bool ShouldStop() {
        bool isBraking = false;
        if (Input.GetKey(KeyCode.B)) {
            isBraking = true;
        }
        else {
            isBraking = false;
        }
        return isBraking;
    }
}
