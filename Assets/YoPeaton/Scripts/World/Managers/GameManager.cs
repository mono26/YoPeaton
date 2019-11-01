using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static bool isPaused;
    public static float timeLeft;

   

    //private static int printScore;
    public static void FinishLevel(bool isPlayerVictorious)
    {
        if(isPlayerVictorious)
        {
            SceneManagerTest.LoadNextScene("VictoryScreenScene");
           //printScore = ScoreManager.instance.CalculateFinalScore();
        }
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
    
}
