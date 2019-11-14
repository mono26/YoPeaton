using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    [SerializeField]
    private PlayerCarInput input = null;

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
    /*protected override void FixedUpdate()
    {
       if (ShouldStop()) {
            // DebugController.LogMessage("STOP!");
            GetMovableComponent?.SlowDown();
        }
        else if (ShouldSlowDown()) {
            // DebugController.LogMessage("Slowing down");
            GetMovableComponent?.SlowDown();
        }
        else {
            GetMovableComponent?.SpeedUp();
        }
         lifeTime--;
    }*/
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

    public override void OnCrossWalkEntered(Crosswalk _crossWalk) {
        // DebugController.LogMessage("Entered crosswalk");
        _crossWalk.OnStartedCrossing(this);
    }

    public override void OnCrossWalkExited(Crosswalk _crossWalk) {
        // DebugController.LogMessage("Exited crosswalk");
        base.OnCrossWalkExited(_crossWalk);
    }
}
