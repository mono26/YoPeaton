using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Colision de: " + this.name + ", con: " + collision.name);
        if(collision.CompareTag("PlayerCar"))
        {
            SceneManagerTest.instance.LoadVictory();
        }
    }
}
