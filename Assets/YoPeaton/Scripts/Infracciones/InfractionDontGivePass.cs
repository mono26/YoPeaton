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
        if (AreThereActivePedestrians)
        {
            return true;
        }
        else
        {
            return false;
        }
    }   
}
