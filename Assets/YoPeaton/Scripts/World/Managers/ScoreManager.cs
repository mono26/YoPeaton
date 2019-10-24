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

    private static int score;
    public float countDuration = 2f;

    private static int correctAnswers;
    private static int wrongAnswers;

    private static int infractionsCommited;

    private static int wrongReports;
    private static int correctReports;

    [SerializeField]
    private int scoreForCorrectInputs = 100;

    [SerializeField]
    private int scoreForWrongInputs = 60;

    public static void AddScore(int _score) {
        score =+ _score;
    }

    IEnumerator CountTo(int target)
    {
        int start = score;
        for (float timer = 0; timer < countDuration; timer += Time.deltaTime)
        {
            float progress = timer / countDuration;
            score = (int)Mathf.Lerp(start, target, progress);
            yield return null;
        }
        score = target;
    }

    public int CalculateFinalScore()
    {
        var correctReportScore = correctReports * scoreForCorrectInputs;
        var correctAnswerScore = correctAnswers * scoreForCorrectInputs;

        var wrongReportScore = wrongReports * scoreForWrongInputs;
        var wrongAnswerScore = wrongAnswers * scoreForWrongInputs;

        var InfractionScore = infractionsCommited * 100;

        score = (correctAnswerScore + correctReportScore) - (InfractionScore + wrongAnswerScore + wrongReportScore);
        return score;
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
