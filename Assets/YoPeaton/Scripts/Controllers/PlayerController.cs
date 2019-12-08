using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    [SerializeField]
    public PlayerCarInput Input { get; private set; }

    //Awake is always called before any Start functions
    protected override void Awake()
    {
        base.Awake();
        if (!Input)
        {
            Input = GetComponent<PlayerCarInput>();
        }
    }
    
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pedestrian") || collision.CompareTag("Car"))
        {
            print("¡¡ME CHOQUE CON EL MACHETAZO!!");
            base.OnEntityCollision();
            GameManager.didPlayerLose = true;
            CanvasManager._instance.GenerateFeedback("Crash");
            CanvasManager._instance.ActivateCheckOrCross(false);
            StartCoroutine(CrashCR());
        }
    }
    protected override bool ShouldStop() 
    {
        //NUEVO PUNTO PARA EL LLAMADO DE LA COLISION, PARA NO DEPENDER DEL CHECK DEL ENTITY CONTROLLER//
        bool stop = false;
        collisionCheckResult = CheckForCollision();
        if (IsOnTheStreet && collisionCheckResult.collided && collisionCheckResult.otherEntity.IsOnTheStreet)
        {
            stop = true;
            DebugController.LogErrorMessage("Player should stop!");
            base.OnEntityCollision();
            OnEntityCollision();
            collisionCheckResult.otherEntity?.OnEntityCollision();
            if (collisionCheckResult.otherEntity.CompareTag("Pedestrian"))
            {
                collisionCheckResult.otherEntity?.GetComponent<PedestrianAnimator>().OnPublicPedestrianRunOver();
            }
        }
        return stop;
        // return input.IsBraking;
    }

    protected override bool ShouldSlowDown() {
        bool slowdown = false;
        if (Input.IsBraking) {
            slowdown = true;
        }
        return slowdown;
    }

    public override void OnCrossWalkEntered(Crosswalk _crossWalk) {
        DebugController.LogMessage("Player entered crosswalk");
        _crossWalk.OnStartedCrossing(this);
    }

    public override void OnCrossWalkExited(Crosswalk _crossWalk) {
        DebugController.LogMessage("Player exited crosswalk");
        base.OnCrossWalkExited(_crossWalk);
    }


    public override void OnEntityCollision()
    {
        print("¡¡ME CHOQUE!!");
        base.OnEntityCollision();
        GameManager.didPlayerLose = true;
        CanvasManager._instance.GenerateFeedback("Crash");
        CanvasManager._instance.ActivateCheckOrCross(false);
        StartCoroutine(CrashCR());
        //ShouldStop();

        // Game over.
    }

    IEnumerator CrashCR()
    {
        yield return CanvasManager._instance.DisapearFeedbackText();
        SceneManagerTest.LoadNextScene("VictoryScreenScene");
    }

    protected override bool ShouldSpeedUp()
    {
        bool speedUp = false;
        if (Input.IsAccelerating) {
            speedUp = true;
        }
        return speedUp;
    }
}
