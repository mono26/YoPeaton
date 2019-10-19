using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static bool isPaused;
    public static void FinishLevel(bool isPlayerVictorious)
    {
        if(isPlayerVictorious)
        {

        }
        else
        {

        }
    } 

    public static void PauseGame()
    {
        isPaused = !isPaused;
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
