using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    void Update()
    {
        var currentDirection = this.gameObject.GetComponent<CarMovement>().currentDirection; 
        Debug.LogWarning("Current Direction: " + currentDirection);
    }
}
