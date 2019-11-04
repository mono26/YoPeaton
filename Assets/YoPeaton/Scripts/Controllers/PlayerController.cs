using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    public static float lifeTime = 500f;
    [SerializeField]
    private PlayerCarInput input = null;

    private bool isCrossingACrossWalk = false;
    private float colliderRadius;

    //Awake is always called before any Start functions
    protected override void Awake()
    {
        base.Awake();
        if (!input)
        {
            input = GetComponent<PlayerCarInput>();
        }
    }

    //MACHETAZO PARA EL PUNTAJE//
    private void Update()
    { 
        lifeTime--;
    }
    protected override bool ShouldStop() {
        return false;
        // return input.IsBraking;
    }

    protected override bool ShouldSlowDown() {
        bool slowdown = false;
        if (IsThereAObstacleUpFront() || input.IsBraking) {
            slowdown = true;
        }
        return slowdown;
    }

    public override bool IsCrossingACrossWalk() {
        return isCrossingACrossWalk;
    }

    public override void OnCrossWalkEntered(Crosswalk _crossWalk) {
        // DebugController.LogMessage("Entered crosswalk");
        isCrossingACrossWalk = true;
    }

    public override void OnCrossWalkExited(Crosswalk _crossWalk) {
        // DebugController.LogMessage("Exited crosswalk");
        isCrossingACrossWalk = false;
    }
}
