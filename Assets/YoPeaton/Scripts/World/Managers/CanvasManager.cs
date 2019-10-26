using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager _instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

    public float duration = 2f;

    private static int MaxScore;
    public float countDuration = 2f;

    public int[] scoresToCountTo = new int[7];
    public Text[] textsForScores = new Text[7];

    [SerializeField]
    private Canvas baseCanvas;
    [SerializeField]
    private Canvas pausaCanvas;
    [SerializeField]
    public Canvas optInButtonCanvas;
    [SerializeField]
    public Canvas identifyCrossingCanvas;

    private string currentActiveCanvas;

    private int startCount = 0;

    int target;
    public float currentScore { get; private set; }

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

    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0;

        //scoresToCountTo = new int[7];
        //Desastivar todos los otros canvas
        pausaCanvas.enabled = false;
        optInButtonCanvas.enabled = false;
        identifyCrossingCanvas.enabled = false;

        //Activar unicamente el base
        baseCanvas.enabled = true;
        currentActiveCanvas = baseCanvas.name;
    }

    /*private void Update()
    {
        Debug.LogError("Timescale: " + Time.timeScale);
    }*/

    //MANEJO DE CANVAS//
    public void ActivatePauseCanvas()
    {
        if (GameManager.isPaused)
        {
            baseCanvas.enabled = true;
            pausaCanvas.enabled = false;
        }
        else 
        {
            baseCanvas.enabled = false;
            pausaCanvas.enabled = true;
        }
    }

    public void FillTextArray()
    {
        //Llenar el array de textos para poder hacer un ciclo para llenar los textos con los scores con el countto//
        if (SceneManagerTest.GetCurrentScene() == "VictoryScreenScene")
        {

            textsForScores[0] = GameObject.Find("TiempoCounter").GetComponent<Text>();
            textsForScores[1] = GameObject.Find("RespuestasCorrectasCounter").GetComponent<Text>();
            textsForScores[2] = GameObject.Find("RespuestasIncorrectasCounter").GetComponent<Text>();
            textsForScores[3] = GameObject.Find("ReportesCorrectosCounter").GetComponent<Text>();
            textsForScores[4] = GameObject.Find("ReportesIncorrectosCounter").GetComponent<Text>();
            textsForScores[5] = GameObject.Find("InfraccionesCounter").GetComponent<Text>();
            textsForScores[6] = GameObject.Find("TotalCounter").GetComponent<Text>();

            ScoreManager.instance.TestFillScoreArray();
            //FILL SCORES ARRAY FROM SCORE MANAGER FOR COUNTER//
            scoresToCountTo[0] = ScoreManager.timeScore;
            scoresToCountTo[1] = ScoreManager.correctAnswerScore;
            scoresToCountTo[2] = ScoreManager.wrongAnswerScore;
            scoresToCountTo[3] = ScoreManager.correctReportScore;
            scoresToCountTo[4] = ScoreManager.wrongReportScore;
            scoresToCountTo[5] = ScoreManager.InfractionScore;
            scoresToCountTo[6] = ScoreManager.finalScore;

           /* Debug.LogError(scoresToCountTo[0]);
            Debug.LogError(scoresToCountTo[1]);
            Debug.LogError(scoresToCountTo[2]);
            Debug.LogError(scoresToCountTo[3]);
            Debug.LogError(scoresToCountTo[4]);
            Debug.LogError(scoresToCountTo[5]);
            Debug.LogError(scoresToCountTo[6]);*/



        }
    }

    public void ActivateSpecificCanvas(string canvasToActivate)
    {
        if (canvasToActivate == "OptInCanvas")
        {
            optInButtonCanvas.enabled = true;
        }
        if (canvasToActivate == "SignalIdentificationCanvas")
        {
              identifyCrossingCanvas.enabled = true;
              optInButtonCanvas.enabled = false;
        }
    }

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

    public void BackToBaseCanvas()
    {
        baseCanvas.enabled = true;
    }

    public void StartCountSequence()
    {
        StartCoroutine(Seq());
    }

    private IEnumerator Seq()
    {
        yield return StartCoroutine(CountTime(scoresToCountTo[0]));
        yield return StartCoroutine(CountCorrectAnswers(scoresToCountTo[1]));
        //yield return StartCoroutine(CountTo());
    }
    /*IEnumerator CountCorrectAnswers(int target)
    {
        int start = score;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            score = (int)Mathf.Lerp(start, target, progress);
            yield return null;
        }
        score = target;
    }*/

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
    }

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
            for(int j = 0; j<MaxScore; i++)
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
