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

    protected override void Update()
    {
        if (ShouldStop())
        {
            // DebugController.LogMessage("STOP!");
            GetMovableComponent?.SlowDown();
        }
        else if (ShouldSlowDown())
        {
            // DebugController.LogMessage("Slowing down");
            GetMovableComponent?.SlowDown();
        }
        else if (ShouldStopInmediatly())
        {
            //DebugController.LogMessage("¡Colision!");
            GetMovableComponent?.ShouldInmediatlyStop();
        }
        else
        {
            GetMovableComponent?.SpeedUp();
        }
    }

    protected override bool ShouldStop() {

        return false;
        // return input.IsBraking;
    }

    public bool ShouldStopInmediatly()
    {
        if (IsThereAObstacleUpFront())
            return true;
        else
            return false;
    }

    protected override bool ShouldSlowDown() {
        bool slowdown = false;
        if (input.IsBraking) {
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
