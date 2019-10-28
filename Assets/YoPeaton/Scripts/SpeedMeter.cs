using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField]
    private GameObject speedIndicator;
    
    private Text speedText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void UpdateSpeed(float speed)
    {
        speedText.text = speed.ToString();
    }

}
