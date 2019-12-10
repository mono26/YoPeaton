using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Colision de: " + this.name + ", con: " + collision.name);
        if(collision.gameObject.name == "PlayerCar_PFB Variant")
        {
            SceneManagerTest.instance.LoadVictory();
        }
        if (this.gameObject.name== "Goal_Tutorial" && collision.gameObject.name == "PlayerCar_PFB Variant")
        {
            SceneManagerTest.instance.LoadScene("TestScene 2");
        }
    }
}
