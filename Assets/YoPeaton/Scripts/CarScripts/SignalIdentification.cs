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


    //Funciones genericas (se pueden desplazar al script de controlador una vez se cree)
    private void Start()
    {

    }
    //Funciones
    private string OnTriggerEnter2D(Collider2D collision)
    {
        //Cada que entra a una señal, actia el canvas que le permite decidir si la quiere identificar o no//
        //Tambien defne la repuesta correcta como el nobre del cruce en el que acaba de entrar//
        CanvasManager._instance.ActivateSpecificCanvas("SignalIdentificationCanvas");
        correctAnswer = collision.name;
        return correctAnswer;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Se desactiva el canvas de identificar la señal una vez sale de ella//
        CanvasManager._instance.BackToBaseCanvas();
        //CanvasManager._instance.optInButtonCanvas.enabled = false;
        correctAnswer = null;
    }

    public void ActivateCrossingIdentificationCanvas()
    {
        CanvasManager._instance.ActivateSpecificCanvas("SignalIdentificationCanvas");
        /*CanvasManager._instance.identifyCrossingCanvas.enabled = true;
        CanvasManager._instance.optInButtonCanvas.enabled = false;*/
    }

    public void CheckAnswer(string selectedAnswer)
    {
        if (correctAnswer == selectedAnswer)
        {
            ScoreManager.AddScore(50);
            Debug.Log("Escogiste la respuesta correcta");
        }

        else
        {
            Debug.LogError("Te equivocaste, wey");
        }
        CanvasManager._instance.BackToBaseCanvas();
    }

}
