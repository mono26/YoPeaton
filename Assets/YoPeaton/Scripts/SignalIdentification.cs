using System.Collections;
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


    //Funciones
    private string OnTriggerEnter2D(Collider2D collision)
    {
        //Cada que entra a una señal, actia el canvas que le permite decidir si la quiere identificar o no//
        //Tambien defne la repuesta correcta como el nobre del cruce en el que acaba de entrar//
        if(collision.tag == "CrossWalk")
        {
            //CanvasManager._instance.ActivateSpecificCanvas("SignalIdentificationCanvas");
            CanvasManager._instance.ActivateSpecificCanvas("OptInCanvas");
            correctAnswer = collision.GetComponent<Crosswalk>().GetCrossWalkType.ToString();
            return correctAnswer;
        }
        else
        {
            return null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        //Se desactiva el canvas de identificar la señal una vez sale de ella//
        CanvasManager._instance.BackToBaseCanvas();
        //CanvasManager._instance.optInButtonCanvas.enabled = false;
        correctAnswer = null;
    }

    public void AcceptSignalIdentification()
    {
        //El tiempo se pone lento//
        Time.timeScale = 0.5f;
        //Debug.LogWarning("Accepted");
        CanvasManager._instance.ActivateSpecificCanvas("SignalIdentificationCanvas");
    }
    public void DeclineSignalIdentification()
    {
        //Debug.LogWarning("Declined");
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
            //CanvasManager._instance.testAnswerText.text = "Respuesta: Correcta!";
            ScoreManager.instance.AddAnswer(true);
            CanvasManager._instance.ActivateCheckOrCross(true);
            //Debug.Log("Escogiste la respuesta correcta");
        }

        else
        {
            ScoreManager.instance.AddAnswer(false);
            CanvasManager._instance.ActivateCheckOrCross(false);
            //CanvasManager._instance.testAnswerText.text = "Respuesta: Incorrecta :(";
            //Debug.LogWarning("Te equivocaste, wey");
        }
        CanvasManager._instance.crosswalkTypesButtons.SetActive(false);
        CanvasManager._instance.BackToBaseCanvas();
    }

}
