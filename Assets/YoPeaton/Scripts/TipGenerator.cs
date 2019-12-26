using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipGenerator : MonoBehaviour
{
    [SerializeField]
    private string[] tips;
    private Text tipText;

    void Awake()
    {
        tipText = this.GetComponent<Text>();
        int tipIndex = Random.Range(0, tips.Length);
        tipText.text = tips[tipIndex];
    }
}
