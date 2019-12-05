using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrossedWithPedestrianInfraction", menuName = "Cross With Pedestrian Infraction")]
public class InfractionCrossingWithPedestrian : InfractionSO
{
    /*Infraccion de no detenerse cuando hay una persona esperando o acercandose mucho al paso peatonal, cuando se toque el carro
    que se va a evaluar, este chequea si esta en colision con un triger de vehicleHotZone de este pase y chequea el padre del trigger 
    a ver si dentro del collider de peatones de ese mismo padre hay peaton, si lo hay, y no se detiene (llega al collider del pase peatonal)
    Se levanta una infraccion con el ruletocheck*/


    protected override bool RuleToCheck()
    {
        if (evaluatedObject.GetComponent<EntityController>().GetisThisCarCrossingWithPedestrian)
        {
            CanvasManager._instance.GenerateFeedback("NoWayReportCorrect");

            ScoreManager.instance.AddReport(true);
            CanvasManager._instance.ActivateCheckOrCross(true);
            Debug.Log("Ya habia alguien pasando"); Debug.Log("REPORTE CORRECTO, HABIA ALGUIEN CRUZANDO, ROMPIO LA LEY");
            return true;
        }
        else
        {
            ScoreManager.instance.AddReport(false);
            CanvasManager._instance.ActivateCheckOrCross(false);
            Debug.Log("NO HABIA NADIE"); Debug.Log("REPORTE INCORRECTO, NO HABIA NADIE AHÍ, PODIA PASAR");
            CanvasManager._instance.GenerateFeedback("WrongReport");
            return false;
        }
    }   
}
