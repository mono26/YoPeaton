using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            if (this.gameObject.name == "Goal_Tutorial")
            {
                GameManager.SetTutorialPref();
                SceneManagerTest.instance.LoadScene(GameManager.gameScene);
            }
            else
            {
                SceneManagerTest.instance.LoadVictory();
            }
        }
    }
}
