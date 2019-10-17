using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    [SerializeField]
    private PlayerCarInput input = null;

    protected void Awake() {
        if (!input) {
            input = GetComponent<PlayerCarInput>();
        }
    }

    protected override bool ShouldStop() {
        return input.IsBraking;
    }
}
