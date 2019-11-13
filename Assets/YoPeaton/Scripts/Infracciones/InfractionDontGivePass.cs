using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DontGivePassInfraction", menuName = "Don't Give Way Infraction")]
public class InfractionDontGivePass : InfractionSO
{
    /*Infraccion de no detenerse cuando hay una persona esperando o acercandose mucho al paso peatonal, cuando se toque el carro
    que se va a evaluar, este chequea si esta en colision con un triger de vehicleHotZone de este pase y chequea el padre del trigger 
    a ver si dentro del collider de peatones de ese mismo padre hay peaton, si lo hay, y no se detiene (llega al collider del pase peatonal)
    Se levanta una infraccion con el ruletocheck*/


    protected override bool RuleToCheck()
    {
        var rbObject = evaluatedObject.GetComponent<Rigidbody2D>();
        var controller = evaluatedObject.GetComponent<AIController>();
        if (AreThereCrossingPedestrians && controller.GetCurrentState.ToString() != "WaitingAtCrossWalk")
        {
            CanvasManager._instance.GenerateFeedback("NoWayReportCorrect");
            Debug.LogError("ESTADO DEL AI CONTROLLER: " + controller.GetCurrentState.ToString());
            ScoreManager.instance.AddReport(true);
            CanvasManager._instance.ActivateCheckOrCross(true);
            Debug.Log("No Diste Via"); Debug.Log("REPORTE CORRECTO, HABIA ALGUEIN, ROMPIO LA LEY");
            //CanvasManager._instance.testReportText.text = "Reporte: Correcto";
            //CanvasManager._instance.StartResetCanvasCoroutine();
            return true;
        }
        else
        {
            ScoreManager.instance.AddReport(false);
            CanvasManager._instance.ActivateCheckOrCross(false);
            Debug.Log("BIEN NO HABIA NADIE"); Debug.Log("REPORTE INCORRECTO, NO HABIA NADIE AHÍ, PODIA PASAR");
            CanvasManager._instance.GenerateFeedback("WrongReport");
            //CanvasManager._instance.testReportText.text = "Reporte: Incorrecto";
            //CanvasManager._instance.StartResetCanvasCoroutine();
            return false;
        }
    }   
}
