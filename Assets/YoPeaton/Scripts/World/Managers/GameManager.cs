using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static bool isPaused = false;
    public static float timeLeft;
    public static bool didPlayerLose = false;
    public static int timesTutorialFinished = 0;
    public static int timesAnsweredCorrectly = 0;

    public const string gameScene = "GameScene";
    public const string tutorialScene = "Tutorial";
    public const string victoryScene = "VictoryScreenScene";
    public const string menuScene = "MainMenu";

    //private static int printScore;
    public static void FinishLevel()
    {
        if(SceneManagerTest.GetCurrentScene() != GameManager.victoryScene)
            SceneManagerTest.LoadNextScene(GameManager.victoryScene);
           //printScore = ScoreManager.instance.CalculateFinalScore();
        
    } 

    public static void PauseGame()
    {
        isPaused = !isPaused;
        Debug.LogWarning("Is Paused: " + isPaused);
        if(isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public static void SetTutorialPref()
    {
        PlayerPrefs.SetInt("timestutorialfinished", timesTutorialFinished + 1);
    }
    public static int GetTutorialPref()
    {
        return PlayerPrefs.GetInt("timestutorialfinished");
    }

    public static void SetSignalIdPref()
    {
        PlayerPrefs.SetInt("timesAnsweredCorrectly", timesAnsweredCorrectly + 1);
    }
    public static int GetSignalIdPref()
    {
        return PlayerPrefs.GetInt("timesAnsweredCorrectly");
    }

}
