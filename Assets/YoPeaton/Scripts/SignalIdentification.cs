﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalIdentification : MonoBehaviour
{

    /*Detección de contacto con un paso peatonal
   los pasos peatonales deben tener un collider en modo trigger
   una vez detecte una colisión dispara un canvas inactivo 
   que muestre inicialmente un botón (optInButton) para que el usuario decida
   si quiere identificar el paso y guarda el nombre del paso con
   el que colisiono, si hunde el botón, abre otro canvas
   que le muestre las opciones, una vez el usuario decida, se
   compara el nombre del paso con la respuesta que seleccionó el
   usuario para verificar si es correcta, si lo es se añaden puntos
   si no, se le restan, si ignora el boton para identificar no tiene
   beneficio ni perjuicio por no responder la identificación*/

    //Variables
    //CorrectAnswer es el nombre del cruce porque acaba de pasar.
    [SerializeField]
    private string correctAnswer;
    [SerializeField]
    private string selectedName;

    [SerializeField]
    private bool canAnswer = true;

    [SerializeField]
    private int correctAnswersQt = 0;

    private void Start()
    {
        canAnswer = true;
    }
    //Funciones
    private string OnTriggerEnter2D(Collider2D collision)
    {

        //Cada que entra a una señal, actia el canvas que le permite decidir si la quiere identificar o no//
        //Tambien defne la repuesta correcta como el nobre del cruce en el que acaba de entrar//
        if (collision.tag == "CrossWalk" && canAnswer == true && correctAnswersQt < 3)
        {
            Time.timeScale = 0f;
            Debug.Log("Time Scale: " + Time.timeScale);
            //CanvasManager._instance.ActivateSpecificCanvas("SignalIdentificationCanvas");
            CanvasManager._instance.ActivateSpecificCanvas("OptInCanvas");
            correctAnswer = collision.GetComponent<Crosswalk>().GetCrossWalkType.ToString();
            StartCoroutine(ChangeCanAnswerValueCR());
            canAnswer = false;
            return correctAnswer;
        }
        else
        {
            return null;
        }
    }

    IEnumerator ChangeCanAnswerValueCR()
    {
        yield return new WaitForSeconds(3);
        canAnswer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Time.timeScale =1f;
        //Debug.Log("Time Scale: " + Time.timeScale);
        //Se desactiva el canvas de identificar la señal una vez sale de ella//
        CanvasManager._instance.BackToBaseCanvas();
        //CanvasManager._instance.optInButtonCanvas.enabled = false;
        correctAnswer = null;
    }

    public void AcceptSignalIdentification()
    {
        //El tiempo se pone lento//
        Time.timeScale = 0f;
        //Debug.LogWarning("Accepted");
        CanvasManager._instance.ActivateSpecificCanvas("SignalIdentificationCanvas");
    }
    public void DeclineSignalIdentification()
    {
        //Debug.LogWarning("Declined");
        Time.timeScale = 1;
        CanvasManager._instance.BackToBaseCanvas();
    }

    public void ActivateCrossingIdentificationCanvas()
    {
        CanvasManager._instance.ActivateSpecificCanvas("SignalIdentificationCanvas");
        /*CanvasManager._instance.identifyCrossingCanvas.enabled = true;
        CanvasManager._instance.optInButtonCanvas.enabled = false;*/
    }

    public void CheckAnswer(string selectedAnswer)
    {
        //El tiempo se pone normal//
        Time.timeScale = 1f;
        selectedName = selectedAnswer;
        if (correctAnswer == selectedAnswer)
        {
            CanvasManager._instance.GenerateFeedback("CorrectAnswer");
            correctAnswersQt++;
            ScoreManager.instance.AddAnswer(true);
            CanvasManager._instance.ActivateCheckOrCross(true);
        }

        else
        {
            ScoreManager.instance.AddAnswer(false);
            CanvasManager._instance.ActivateCheckOrCross(false);
            CanvasManager._instance.GenerateFeedback("WrongAnswer");
        }
        CanvasManager._instance.crosswalkTypesButtons.SetActive(false);
        CanvasManager._instance.BackToBaseCanvas();
    }

}
