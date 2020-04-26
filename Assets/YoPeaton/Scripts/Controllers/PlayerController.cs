using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    [SerializeField]
    public PlayerCarInput Input { get; private set; }

    #region Unity functions
    //Awake is always called before any Start functions
    protected override void Awake()
    {
        base.Awake();
        Input = GetComponent<PlayerCarInput>();
        SignalIdentification signaler = GetComponent<SignalIdentification>();
        if (signaler)
        {
            signaler.onQuestionAsk += OnQuestionAsk;
        }
    }
    
    private void OnDestroy() 
    {
        SignalIdentification signaler = GetComponent<SignalIdentification>();
        if (signaler)
        {
            signaler.onQuestionAsk -= OnQuestionAsk;
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pedestrian") || collision.CompareTag("Car"))
        {
            DebugController.LogMessage("¡¡ME CHOQUE CON EL MACHETAZO!!");
            OnEntityCollision(collision.gameObject.GetComponent<EntityController>());
            GameManager.didPlayerLose = true;
            CanvasManager._instance.GenerateFeedback("Crash");
            CanvasManager._instance.ActivateCheckOrCross(false);
            StartCoroutine(CrashCR());
        }
    }
    #endregion

    protected override void SetRandomEntityType()
    {
        SetEntitySubType = EntitySubType.BlueCar;
    }

    protected override bool ShouldStop() 
    {
        //NUEVO PUNTO PARA EL LLAMADO DE LA COLISION, PARA NO DEPENDER DEL CHECK DEL ENTITY CONTROLLER//
        bool stop = false;
        RaycastCheckResult collisionCheck = HasCollided();
        if (collisionCheck.collided && collisionCheck.otherEntity.IsOnTheStreet)
        {
            stop = true;
            DebugController.LogErrorMessage("Player should stop!");
            OnEntityCollision(collisionCheck.otherEntity);
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

    #region Crosswalk methods
    public override void OnCrossWalkEntered(ICrossable _crossWalk) 
    {
        DebugController.LogMessage("Player entered crosswalk");
        // Si hay peatones cruzando sacar lo de las infracciones
        Crosswalk cross = ((Crosswalk)_crossWalk);
        if (cross.GetNumberOfCrossingPedestrians > 0)
        {
            if (!cross.CanCrossIfPathIsFree(this))
            {
                ScoreManager.instance.AddInfraction();
                CanvasManager._instance.ActivateCheckOrCross(false);
                CanvasManager._instance.GenerateFeedback("CrossWithPedestrian");
            }
        }
        OnStartedCrossing(_crossWalk);
        _crossWalk.OnStartedCrossing(this);
    }

    public override void OnCrossWalkExited(ICrossable _crossWalk) 
    {
        DebugController.LogMessage("Player exited crosswalk");
        _crossWalk.OnFinishedCrossing(this);
    }
    #endregion

    public override void OnEntityCollision(EntityController _otherEntity)
    {
        print("¡¡ME CHOQUE!!");
        base.OnEntityCollision(_otherEntity);
        if (_otherEntity && _otherEntity.gameObject.CompareTag("Pedestrian"))
        {
            _otherEntity.GetComponent<AIController>().SwitchToState(AIState.Waiting);
            _otherEntity.GetComponent<PedestrianAnimator>().OnPublicPedestrianRunOver();
        }
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
        SceneManagerTest.LoadNextScene(GameManager.victoryScene);
        Time.timeScale = 1;
    }

    protected override bool ShouldSpeedUp()
    {
        bool speedUp = false;
        if (Input.IsAccelerating) {
            speedUp = true;
        }
        return speedUp;
    }

    private void OnQuestionAsk()
    {
        GetMovableComponent.SlowDownByPercent(100.0f);
    }

    public override void OnIntersectionEntered(ICrossable _crossWalk)
    {

    }

    public override void OnIntersectionExited(ICrossable _intersection)
    {

    }
}
