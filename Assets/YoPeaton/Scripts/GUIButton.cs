using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIButton : MonoBehaviour
{
    [SerializeField]
    private float SCALE_FACTOR;
    private GameObject button;
    private Transform buttonTransform;
    private bool mouseOnTop;
    private Vector3 restScale;

    void Start()
    {
        button = this.gameObject;
        buttonTransform = button.GetComponent<Transform>();
        restScale = buttonTransform.localScale;
    }

    private void ScaleButton()
    {
        if (mouseOnTop)
        {
            buttonTransform.localScale += restScale * SCALE_FACTOR;
        } else
        {
            buttonTransform.localScale = restScale;
        }
    }

    void Update()
    {
        //TODO: Raycast and call ScaleButton()           
    }
}
