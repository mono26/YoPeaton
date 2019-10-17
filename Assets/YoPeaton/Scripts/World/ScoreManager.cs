using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int score;

    public static void AddScore(int _score) {
        score =+ _score;
    }
}
