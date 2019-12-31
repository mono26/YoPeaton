using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using System;

public class CanvasManager : MonoBehaviour
{

    #region Variables
    public static CanvasManager _instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

    public float duration = 1f;

    [SerializeField]
    private GameObject FeedBackTextGO;
    [SerializeField]
    private Text FeedBackText;

    private static int MaxScore;
    public float countDuration = 1f;

    public int[] scoresToCountTo = new int[7];
    public Text[] textsForScores = new Text[7];



    /*
    public Text testReportText;
    public Text testAnswerText;
    */

    [Header("Botones Menu: ")]
    public Button playBtn;
    public Button quitBtn;
    public Button optionsBtn;

    [Header("Botones Victorua: ")]
    public Button retryBtn;
    public Button quitFinalBtn;

    [Header("Objetos Escena: ")]
    [SerializeField]
    private GameObject hudPanel;
    [SerializeField]
    private GameObject scoreContainer;
    [SerializeField]
    private GameObject speedMeter;
    [SerializeField]
    private GameObject brakeBtn;
    [SerializeField]
    private GameObject accelBtn;
    [SerializeField]
    private GameObject pauseBtn;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    public GameObject crosswalkGuessHUD;
    [SerializeField]
    public GameObject crosswalkTypesButtons;
    [SerializeField]
    public GameObject crosswalkQuestionButtons;
    [SerializeField]
    public Image checkAndCrossImg;
    [SerializeField]
    public Text debugText;

    [SerializeField]
    public GameObject checkAndCrossGO;

    [SerializeField]
    public Canvas wholeCanvas;

    [SerializeField]
    public Animator checkAndCrossAnimator;
    [SerializeField]
    public Sprite checkSprite;
    [SerializeField]
    public Sprite crossSprite;

    [SerializeField]
    private Image speedImage;
    [SerializeField]
    private PlayerController player;
    private float fillPercent;

    public Text timeLeftText;


    private Button botonVolver;


    private string currentActiveCanvas;

    private int startCount = 0;

    int target;
    public float currentScore { get; private set; }

    #endregion

    #region Singleton Definition
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (_instance == null)

            //if not, set instance to this
            _instance = this;

        //If instance already exists and it's not this:
        else if (_instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {

        // FillReferences();
        //playerGO = GameObject.Find("PlayerCar_PFB Variant");
        checkAndCrossImg.enabled = false;
        currentScore = 0;

        //Activar unicamente el base
        hudPanel.SetActive(true);
        currentActiveCanvas = hudPanel.name;

        //scoresToCountTo = new int[7];
        //Desastivar todos los otros canvas
        pausePanel.SetActive(false);
        crosswalkGuessHUD.SetActive(false);
        crosswalkTypesButtons.SetActive(false);
        //crosswalkQuestionButtons.SetActive(false);

    }

    public void TurnOffHUD()
    {
        wholeCanvas.enabled = false;
        /*scoreContainer.SetActive(false);
        speedMeter.SetActive(false);
        brakeBtn.SetActive(false);
        pauseBtn.SetActive(false);
        accelBtn.SetActive(false);*/
    }

    public void TurnOnHUD()
    {
        wholeCanvas.enabled = true;
        /*scoreContainer.SetActive(true);
        speedMeter.SetActive(true);
        brakeBtn.SetActive(true);
        pauseBtn.SetActive(true);
        accelBtn.SetActive(true);*/
    }
    public void StartReplacementMethod()
    {
        checkAndCrossImg.enabled = false;
        currentScore = 0;

        //Activar unicamente el base
        hudPanel.SetActive(true);
        currentActiveCanvas = hudPanel.name;

        //scoresToCountTo = new int[7];
        //Desastivar todos los otros canvas
        pausePanel.SetActive(false);
        crosswalkGuessHUD.SetActive(false);
        crosswalkTypesButtons.SetActive(false);
        //crosswalkQuestionButtons.SetActive(false);
    }

    public void FillMenuBtns()
    {
        Debug.LogError("VOY A LLENAR LOS BOTONES DEL MENU");
        playBtn = GameObject.Find("Button_Play").GetComponent<Button>();
        quitBtn = GameObject.Find("Button_Quit").GetComponent<Button>();
        optionsBtn = GameObject.Find("Button_Options").GetComponent<Button>();
    }

    public void FillVictoryButtons()
    {
        Debug.LogError("VOY A LLENAR LOS BOTONES DEL VICTORY");
        retryBtn = GameObject.Find("BotonVolver").GetComponent<Button>();
        quitFinalBtn = GameObject.Find("BotonExit").GetComponent<Button>();
    }

    public void FillVictoryButtonMethods()
    {
        retryBtn.onClick.AddListener(delegate { playBtnClicked(); });
        quitFinalBtn.onClick.AddListener(delegate { quitBtnClicked(); });
    }

    public void FillMenuBtnsMethods()
    {
        Debug.LogError("VOY A LLENAR LOS METODOS DE LOS BOTONES");

        playBtn.onClick.AddListener(delegate { playBtnClicked(); });
        quitBtn.onClick.AddListener(delegate { quitBtnClicked(); });
        optionsBtn.onClick.AddListener(delegate { optionsBtnClicked(); });
    }

    private void optionsBtnClicked()
    {
        Debug.LogError("Hundi opciones");
    }

    private void quitBtnClicked()
    {
        Application.Quit();
        Debug.LogError("Hundi quit");
    }

    private void playBtnClicked()
    {
        int timestutorialfinished = GameManager.GetTutorialPref();
        //PASO AL TUTORIAL//
        if (timestutorialfinished > 0)
        {
            SceneManagerTest.LoadNextScene("TestScene 2");
            Debug.LogError("Hundi play y vamos al juego");
        }
        else if (timestutorialfinished == 0)
        {
            SceneManagerTest.LoadNextScene("Tutorial");
            Debug.LogError("Hundi play y vamos al tutorial");
        }

        //METODO DE LLAMADO BASICO//
        /*SceneManagerTest.LoadNextScene("TestScene 2");
        Debug.LogError("Hundi play y vamos al juego");*/
    }

    public void BackToMenuBtnClicked()
    {
        SceneManagerTest.LoadNextScene("MainMenu");
        Debug.LogError("Hundi quit desde la pausa");
    }
    public void ContinueBtnClicked()
    {
        DeactivatePauseCanvas();

        GameManager.PauseGame();
    }

    public void AssignDebugText()
    {
        //debugText = GameObject.Find("DebugText").GetComponent<Text>();
    }
    public void FillReferences()
    {
        DebugController.LogMessage("VOY A LLENAR LAS REFERENCIAS");
            speedMeter = this.transform.GetChild(0).GetChild(1).gameObject;
            player = FindObjectOfType<PlayerController>();
            FeedBackTextGO = GameObject.Find("FeedbackTest");
            FeedBackText = GameObject.Find("FeedbackTest").GetComponent<Text>();
            hudPanel = this.transform.GetChild(0).gameObject; ;
            pausePanel = this.transform.GetChild(1).gameObject;
            crosswalkGuessHUD = this.transform.GetChild(0).GetChild(2).gameObject;
            crosswalkTypesButtons = this.transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
            crosswalkQuestionButtons = this.transform.GetChild(0).GetChild(2).GetChild(2).gameObject;
        //if (checkAndCrossImg == null)
            checkAndCrossImg = GameObject.Find("CheckAndCrossCanvasIMG").GetComponent<Image>();
        //if (checkAndCrossGO == null)
            checkAndCrossGO = GameObject.Find("CheckAndCrossCanvasIMG");
        //if (checkAndCrossAnimator == null)
            checkAndCrossAnimator = GameObject.Find("CheckAndCrossCanvasIMG").GetComponent<Animator>();
        //if (timeLeftText == null)
            timeLeftText = this.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
    }

    private void Update()
    {
        //Debug.LogError("Pause canvas esta activado? " + pausePanel.activeInHierarchy);
        if (SceneManagerTest.GetCurrentScene() == "Tutorial" || SceneManagerTest.GetCurrentScene() == "TestScene 2")
        {
            if (player)
            {
                fillPercent = (player.GetMovableComponent.GetCurrentSpeed / player.GetMovableComponent.GetMaxSpeed);
                //Debug.Log("Fill Percent: "  + fillPercent);
                speedImage.fillAmount = fillPercent;
                var lifeTimeInt = Mathf.RoundToInt(ScoreManager.lifeTime);
                timeLeftText.text = lifeTimeInt.ToString();
            }

        }
        /*if (SceneManagerTest.GetCurrentScene() == "VictoryScreenScene")
        {
            //PONER A QUE BUSQUE EL BOTON CUANDO SEA LA ESCENA DE VICTORIA Y ASIGNARLE EL METODO QUE TIENE QUE LLAMAR POR CODIGO//
            botonVolver = GameObject.Find("BotonVolver").GetComponent<Button>();
        }*/
    }
    #region Manejo De Canvas
    //MANEJO DE CANVAS//
    public void ActivatePauseCanvas()
    {
        print("ACTIVATE PAUSE CANVAS");
            hudPanel.SetActive(false);
            pausePanel.SetActive(true);
        print("Pause Canvas is Active? " + pausePanel.activeInHierarchy);

    }

    public void DeactivatePauseCanvas()
    {
        print("DEACTIVATE PAUSE CANVAS");
        hudPanel.SetActive(!hudPanel.activeInHierarchy);
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
        print("Pause Canvas is Active? " + pausePanel.activeInHierarchy);

    }

    public void ActivateSpecificCanvas(string canvasToActivate)
    {
        if (canvasToActivate == "OptInCanvas")
        {
            crosswalkGuessHUD.SetActive(true);
            crosswalkTypesButtons.SetActive(true);
            crosswalkQuestionButtons.SetActive(false);
        }

    }

    public void BackToBaseCanvas()
    {
        hudPanel.SetActive(true);
        crosswalkTypesButtons.SetActive(false);
        crosswalkQuestionButtons.SetActive(true);
        crosswalkGuessHUD.SetActive(false);
    }

    #endregion

    public void ActivateCheckOrCross(bool isCorrect)
    { 
        if(isCorrect)
        {
            Debug.Log("ACTIVE EL CHECK");
            checkAndCrossImg.enabled = true;
            checkAndCrossImg.sprite = checkSprite;
            checkAndCrossAnimator.enabled = true;
            checkAndCrossAnimator.Play("Expand And Spin", -1, 0);
            StartCoroutine(DisapearCheckOrCross());
        }
        else
        {
            checkAndCrossImg.enabled = true;
            checkAndCrossImg.sprite = crossSprite;
            checkAndCrossAnimator.enabled = true;
            checkAndCrossAnimator.Play("Expand And Spin", -1, 0);
            StartCoroutine(DisapearCheckOrCross());
        }
    }


    public void FillTextArray()
    {
        //Llenar el array de textos para poder hacer un ciclo para llenar los textos con los scores con el countto//
        if (SceneManagerTest.GetCurrentScene() == "VictoryScreenScene")
        {
            textsForScores[0] = GameObject.Find("TiempoCounter").GetComponent<Text>();
            textsForScores[1] = GameObject.Find("RespuestasCorrectasCounter").GetComponent<Text>();
            textsForScores[2] = GameObject.Find("ReportesCorrectosCounter").GetComponent<Text>();
            textsForScores[3] = GameObject.Find("InfraccionesCounter").GetComponent<Text>();
            textsForScores[4] = GameObject.Find("ReportesIncorrectosCounter").GetComponent<Text>();
            textsForScores[5] = GameObject.Find("RespuestasIncorrectasCounter").GetComponent<Text>();
            textsForScores[6] = GameObject.Find("TotalCounter").GetComponent<Text>();

            //FILL SCORES ARRAY FROM SCORE MANAGER FOR COUNTER//
            scoresToCountTo[0] = ScoreManager.timeScore;
            scoresToCountTo[1] = ScoreManager.correctAnswerScore;
            scoresToCountTo[2] = ScoreManager.correctReportScore;
            scoresToCountTo[3] = ScoreManager.InfractionScore;
            scoresToCountTo[4] = ScoreManager.wrongReportScore;
            scoresToCountTo[5] = ScoreManager.wrongAnswerScore;
            scoresToCountTo[6] = ScoreManager.finalScore;

        }
    }
    public void GenerateFeedback(string feedbackType)
    { 
        switch (feedbackType)
        {
            case "NoWayReportCorrect":
                FeedBackText.enabled = true;
                FeedBackText.color = Color.green;
                FeedBackText.text = "No dio paso al peatón.";
                StartCoroutine(DisapearFeedbackText());
                break;
            case "BlockCrossingReportCorrect":
                FeedBackText.enabled = true;
                FeedBackText.color = Color.green;
                FeedBackText.text = "Bloqueando un paso peatonal.";
                StartCoroutine(DisapearFeedbackText());
                break;
            case "WrongAnswer":
                FeedBackText.enabled = true;
                FeedBackText.color = Color.red;
                FeedBackText.text = "¡Respuesta Incorrecta!";
                StartCoroutine(DisapearFeedbackText());
                break;
            case "CorrectAnswer":
                FeedBackText.enabled = true;
                FeedBackText.color = Color.green;
                FeedBackText.text = "¡Respuesta Correcta!";
                StartCoroutine(DisapearFeedbackText());
                break;
            case "WrongReport":
                FeedBackText.enabled = true;
                FeedBackText.color = Color.red;
                FeedBackText.text = "Conduciendo correctamente.";
                StartCoroutine(DisapearFeedbackText());
                break;
            case "Crash":
                FeedBackText.enabled = true;
                FeedBackText.color = Color.red;
                FeedBackText.text = "¡Tienes que manejar con cuidado!";
                StartCoroutine(DisapearFeedbackText());
                break;
            case "CrossWithPedestrian":
                FeedBackText.enabled = true;
                FeedBackText.color = Color.red;
                FeedBackText.text = "¡Debes esperar a que pase el peatón!";
                StartCoroutine(DisapearFeedbackText());
                break;
        }
    }


    public IEnumerator DisapearFeedbackText()
    {
        yield return new WaitForSecondsRealtime(3);
        if (SceneManagerTest.GetCurrentScene() == "Tutorial" || SceneManagerTest.GetCurrentScene() == "TestScene 2")
            FeedBackText.enabled = false;
    }

    IEnumerator DisapearCheckOrCross()
    {
        //Debug.Log("TENGO QUE APAGAR EL CHECK");
        yield return new WaitForSecondsRealtime(2);
        if (SceneManagerTest.GetCurrentScene() == "Tutorial" || SceneManagerTest.GetCurrentScene() == "TestScene 2")
            checkAndCrossImg.enabled = false;
    }


    #region Manejo de Botones

    //BOTONES//

    public void PressAcceptBtn()
    {
        player.GetComponent<SignalIdentification>().AcceptSignalIdentification();
    }

    public void PressDeclineBtn()
    {
        player.GetComponent<SignalIdentification>().DeclineSignalIdentification();
    }

    public void PressAnswerBtn(string selectedAnswer)
    {
        player.GetComponent<SignalIdentification>().CheckAnswer(selectedAnswer);
    }
    public void PressBrakeBtn()
    {
        player.Input.Brake();
    }

    public void ReleaseBrakeBtn()
    {
        player.Input.StopBrake();
    }

    public void PressAccelerateBtn()
    {
        player.Input.Accelerate();
    }

    public void ReleaseAccelerateBtn()
    {
        player.Input.StopAccelerate();
    }

    public void PressPauseBtn()
    {
        ActivatePauseCanvas();
        Debug.LogWarning("Press Pause Button");
        GameManager.PauseGame();


    }

    public void PressLoadBtn()
    {
        //SceneManagerTest.instance.LoadVictory();
        SceneManagerTest.instance.LoadVictory();
    }


    #endregion

    #region Sequencia de corrutinas de conteo

    public void StartResetCanvasCoroutine()
    {
        StartCoroutine(EraseFeedback());
    }

    public void StartCountSequence()
    {
        StartCoroutine(Seq());
    }

    private IEnumerator Seq()
    {
        yield return StartCoroutine(CountTime(scoresToCountTo[0]));
        yield return StartCoroutine(CountCorrectAnswers(scoresToCountTo[1]));
        yield return StartCoroutine(CountCorrectReports(scoresToCountTo[2]));
        yield return StartCoroutine(CountInfractions(scoresToCountTo[3]));
        yield return StartCoroutine(CountWrongReports(scoresToCountTo[4]));
        yield return StartCoroutine(CountWrongAnswers(scoresToCountTo[5]));
        yield return StartCoroutine(CountTotal(scoresToCountTo[6]));
    }

    IEnumerator CountTime(int target)
    {
        int start = MaxScore;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            MaxScore = (int)Mathf.Lerp(start, target, progress);
            textsForScores[0].text = MaxScore.ToString();
            yield return null;
        }
        MaxScore = target;
        textsForScores[0].text = MaxScore.ToString();
    }

    IEnumerator CountCorrectAnswers(int target)
    {
        int start = MaxScore;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            MaxScore = (int)Mathf.Lerp(start, target, progress);
            textsForScores[1].text = MaxScore.ToString();
            yield return null;
        }
        MaxScore = target;
        textsForScores[1].text = MaxScore.ToString();
    }

    IEnumerator CountCorrectReports(int target)
    {
        int start = MaxScore;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            MaxScore = (int)Mathf.Lerp(start, target, progress);
            textsForScores[2].text = MaxScore.ToString();
            yield return null;
        }
        MaxScore = target;
        textsForScores[2].text = MaxScore.ToString();
    }

    IEnumerator CountInfractions(int target)
    {
        int start = MaxScore;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            MaxScore = (int)Mathf.Lerp(start, target, progress);
            textsForScores[3].text = MaxScore.ToString();
            yield return null;
        }
        MaxScore = target;
        textsForScores[3].text = MaxScore.ToString();
    }

    IEnumerator CountWrongReports(int target)
    {
        int start = MaxScore;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            MaxScore = (int)Mathf.Lerp(start, target, progress);
            textsForScores[4].text = MaxScore.ToString();
            yield return null;
        }
        MaxScore = target;
        textsForScores[4].text = MaxScore.ToString();
    }

    IEnumerator CountWrongAnswers(int target)
    {
        int start = MaxScore;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            MaxScore = (int)Mathf.Lerp(start, target, progress);
            textsForScores[5].text = MaxScore.ToString();
            yield return null;
        }
        MaxScore = target;
        textsForScores[5].text = MaxScore.ToString();
    }

    IEnumerator CountTotal(int target)
    {
        int start = MaxScore;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            MaxScore = (int)Mathf.Lerp(start, target, progress);
            textsForScores[6].text = MaxScore.ToString();
            yield return null;
        }
        MaxScore = target;
        textsForScores[6].text = MaxScore.ToString();
    }

    public IEnumerator EraseFeedback()
    {
        yield return new WaitForSeconds(2);
        //Machetazo para testeo, se deben resetear a tiempos diferentes//
        //testAnswerText.text = "Report: null";
        //testReportText.text = "Report: null";
    }
    #endregion

    #region Codigo Comentado
    /*IEnumerator CountTo()
    {
        Debug.LogWarning("CountTo");
        for (int i = 0; i<textsForScores.Length; i++)
        {
            startCount = 0;
            target = scoresToCountTo[i];
            for (float timer = 0; timer < countDuration; timer += Time.deltaTime)
            {
                float progress = timer / countDuration;
                MaxScore = (int)Mathf.Lerp(startCount, target, progress);
                yield return 2f;
            }
            MaxScore = target;
        }
        CountToScore();
        yield return null;
    }
    IEnumerator FillTextArray()
    {
        Debug.LogWarning("FillTextArray");
        textsForScores[0] = GameObject.Find("TiempoCounter").GetComponent<Text>();
        textsForScores[1] = GameObject.Find("RespuestasCorrectasCounter").GetComponent<Text>();
        textsForScores[2] = GameObject.Find("RespuestasIncorrectasCounter").GetComponent<Text>();
        textsForScores[3] = GameObject.Find("ReportesCorrectosCounter").GetComponent<Text>();
        textsForScores[4] = GameObject.Find("ReportesIncorrectosCounter").GetComponent<Text>();
        textsForScores[5] = GameObject.Find("InfraccionesCounter").GetComponent<Text>();
        textsForScores[6] = GameObject.Find("TotalCounter").GetComponent<Text>();
        yield return null;
    }
    IEnumerator FillScoreArray()
    {
        Debug.LogWarning("FillScoreArray");
        ScoreManager.instance.TestFillScoreArray();
        scoresToCountTo[0] = ScoreManager.timeScore;
        scoresToCountTo[1] = ScoreManager.correctAnswerScore;
        scoresToCountTo[2] = ScoreManager.wrongAnswerScore;
        scoresToCountTo[3] = ScoreManager.correctReportScore;
        scoresToCountTo[4] = ScoreManager.wrongReportScore;
        scoresToCountTo[5] = ScoreManager.InfractionScore;
        scoresToCountTo[6] = ScoreManager.finalScore;
        yield return null;
    }
    public void StartSequence()
    {
        Debug.LogWarning("Iniciar Secuencia");
        StartCoroutine(Seq());
    }
    private IEnumerator Seq()
    {
        yield return StartCoroutine(FillScoreArray());
        yield return StartCoroutine(FillTextArray());
        yield return StartCoroutine(CountTo());
    }*/


    public void CountToWhileWaiting()
    {
        Debug.LogWarning("1) Voy a empezar a contar");
        for (int i = 0; i < scoresToCountTo.Length; i++)
        {
            var currentScore = 0;
            Debug.LogWarning("2) Entre al ciclo for");
            MaxScore = scoresToCountTo[i];
            Debug.Log("Current Max Score: " + MaxScore);
            for (int j = 0; j < MaxScore; i++)
            {
                //currentScore++;
                //int ScoreIncrement = Mathf.RoundToInt(Time.deltaTime * countDuration);
                //currentScore += ScoreIncrement;
                //textsForScores[i].text = currentScore.ToString();
                currentScore = j;
                Debug.Log(currentScore);
                Debug.Log(textsForScores.Length);
                //textsForScores[0].text = currentScore.ToString();
            }
        }
    }
    /*public void CountToScore()
    {
        Debug.LogWarning("1) Voy a empezar a contar");
        for(int i = 0; i < scoresToCountTo.Length; i++)
        {
            Debug.LogWarning("2) Entre al ciclo for");
            MaxScore = scoresToCountTo[i];
            Debug.LogWarning("MAX SCORE (Score to Count to): " + i + " , " + MaxScore);
            while (currentScore < MaxScore)
            {
                Debug.LogWarning("3) Entre al if");
                float ScoreIncrement = Time.deltaTime * countDuration;
                currentScore += ScoreIncrement;
                Debug.LogWarning("CURRENT SCORE: " + i + " , " + currentScore);
                textsForScores[i].text= currentScore.ToString();
                if (currentScore > MaxScore)
                {
                    Debug.LogWarning("4) se igualo la cuenta");
                    currentScore = MaxScore;
                }
            }
        }
    }*/
    #endregion
}
