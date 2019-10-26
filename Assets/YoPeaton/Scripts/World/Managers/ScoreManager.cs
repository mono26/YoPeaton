using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public static int finalScore;
    public float countDuration = 2f;

    public static int correctAnswers = 2;
    public static int wrongAnswers = 1;

    public static int infractionsCommited = 3;

    public static int wrongReports = 4;
    public static int correctReports = 5;

    public static int correctReportScore;
    public static int correctAnswerScore;

    public static int wrongReportScore;
    public static int wrongAnswerScore;

    public static int InfractionScore;

    public static int timeScore;


    [SerializeField]
    private static int scoreForCorrectInputs = 100;

    [SerializeField]
    private static int scoreForWrongInputs = 60;



    private void Start()
    {

    }


    public static void AddScore(int _score) {
        finalScore = +_score;
    }


    public static int CalculateFinalScore()
    {
        Debug.LogError("Going to calculate Final Scores");

        correctReportScore = correctReports * scoreForCorrectInputs;
        correctAnswerScore = correctAnswers * scoreForCorrectInputs;

        wrongReportScore = wrongReports * scoreForWrongInputs;
        wrongAnswerScore = wrongAnswers * scoreForWrongInputs;

        InfractionScore = infractionsCommited * 100;

        finalScore = (correctAnswerScore + correctReportScore) - (InfractionScore + wrongAnswerScore + wrongReportScore);

        Debug.LogError("Wrong reports: " + wrongReports + ", wrong report score: " + wrongReportScore);
        Debug.LogError("Wrong answer: " + wrongAnswers + ", wrong answer score: " + wrongAnswerScore);
        Debug.LogError("correct reports: " + correctReports + ", correct report score: " + correctReportScore);
        Debug.LogError("correct answer: " + correctAnswers + ", correct answer score: " + correctAnswerScore);
        Debug.LogError("Total Score: " + finalScore);

        return finalScore;
    }

    public void TestFillScoreArray()
    {
        Debug.LogWarning("TEST FILL SCRE ARRAY");
        correctAnswerScore = 1000;
        correctReportScore = 1200;
        wrongAnswerScore = 600;
        wrongReportScore = 728;
        InfractionScore = 1000;
        timeScore = 5000;
        //finalScore = CalculateFinalScore();
    }
    public void AddReport(bool isReportCorrect)
    {
        if(isReportCorrect)
        {
            correctReports++;
        }
        else
        {
            wrongReports++;
        }
    }

    public void AddInfraction()
    {
        infractionsCommited++;
    }

    public void AddAnswer(bool isAnswerCorrect)
    {
        if (isAnswerCorrect)
        {
            correctAnswers++;
        }
        else
        {
            wrongAnswers++;
        }
    }
}
