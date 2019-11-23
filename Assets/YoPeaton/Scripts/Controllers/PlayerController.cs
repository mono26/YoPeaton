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

    //protected override void Update()
    //{
    //    float deltaTime = Time.deltaTime;
    //    if (ShouldStop())
    //    {
    //        // DebugController.LogMessage("STOP!");
    //        GetMovableComponent?.SlowDownByPercent(100.0f);
    //    }
    //    else if (ShouldSlowDown())
    //    {
    //        // DebugController.LogMessage("Slowing down");
    //        GetMovableComponent?.SlowDown(deltaTime);
    //    }
    //    else
    //    {
    //        GetMovableComponent?.SpeedUp(deltaTime);
    //    }
    //}

    protected override bool ShouldStop() 
    {
        bool stop = false;
        collisionCheckResult = CheckForCollision();
        if (IsOnTheStreet && result.collided && result.otherEntity.IsOnTheStreet)
        {
            stop = true;
            DebugController.LogErrorMessage("Player should stop!");
        }
        return stop;
        // return input.IsBraking;
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
        // _crossWalk.OnStartedCrossing(this);
    }

    public override void OnCrossWalkExited(Crosswalk _crossWalk) {
        // DebugController.LogMessage("Exited crosswalk");
        base.OnCrossWalkExited(_crossWalk);
    }

    public override void OnEntityCollision()
    {
        base.OnEntityCollision();
        // Game over.
    }
}
