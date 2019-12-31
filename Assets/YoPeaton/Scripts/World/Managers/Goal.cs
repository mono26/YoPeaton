﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Colision de: " + this.name + ", con: " + collision.name);
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            if (this.gameObject.name == "Goal_Tutorial")
            {
                GameManager.SetTutorialPref();
                SceneManagerTest.LoadNextScene("GameScene");
            }
            else
            {
                SceneManagerTest.instance.LoadVictory();
            }
        }
    }
}
