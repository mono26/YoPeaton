using UnityEngine.UI;
using System.Collections;
using UnityEngine;

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

    [SerializeField]
    private GameObject hudPanel;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    public GameObject crosswalkGuessHUD;
    [SerializeField]
    public GameObject crosswalkTypesButtons;
    [SerializeField]
    public GameObject crosswalkQuestionButtons;
    [SerializeField]
    public Text testReportText;
    [SerializeField]
    public Text testAnswerText;

    [SerializeField]
    public Image checkAndCrossImg;

    [SerializeField]
    public GameObject checkAndCrossGO;

    [SerializeField]
    public Animator checkAndCrossAnimator;
    [SerializeField]
    public Sprite checkSprite;
    [SerializeField]
    public Sprite crossSprite;

    [SerializeField]
    private Image speedImage;
    [SerializeField]
    private GameObject playerGO;
    private float fillPercent;

    public Text timeLeftText;


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

    private void Update()
    {
        if (SceneManagerTest.GetCurrentScene() == "TestScene" || SceneManagerTest.GetCurrentScene() != "VictoryScreenScene")
        {
            fillPercent = (playerGO.GetComponent<CarMovement>().GetCurrentSpeed / playerGO.GetComponent<CarMovement>().GetMaxSpeed);
            //Debug.Log("Fill Percent: "  + fillPercent);
            speedImage.fillAmount = fillPercent;
            var lifeTimeInt = Mathf.RoundToInt(ScoreManager.lifeTime);
            timeLeftText.text = lifeTimeInt.ToString();
        }
    }
    #region Manejo De Canvas
    //MANEJO DE CANVAS//
    public void ActivatePauseCanvas()
    {
        if (!GameManager.isPaused)
        {
            hudPanel.SetActive(true);
            pausePanel.SetActive(false);
        }
        else
        {
            hudPanel.SetActive(false);
            pausePanel.SetActive(true);
        }
    }

    public void ActivateSpecificCanvas(string canvasToActivate)
    {
        if (canvasToActivate == "OptInCanvas")
        {
            crosswalkGuessHUD.SetActive(true);
        }
        if (canvasToActivate == "SignalIdentificationCanvas")
        {
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
        }
    }

    IEnumerator DisapearFeedbackText()
    {
        yield return new WaitForSecondsRealtime(2);
        if (SceneManagerTest.GetCurrentScene() == "TestScene")
            FeedBackText.enabled = false;
    }

    IEnumerator DisapearCheckOrCross()
    {
        yield return new WaitForSecondsRealtime(2);
        if (SceneManagerTest.GetCurrentScene() == "TestScene")
            checkAndCrossImg.enabled = false;
    }

    #region Manejo de Botones

    //BOTONES//
    public void PressPauseBtn()
    {
        Debug.LogWarning("Press Pause Button");
        GameManager.PauseGame();
        ActivatePauseCanvas();
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
        testAnswerText.text = "Report: null";
        testReportText.text = "Report: null";
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
