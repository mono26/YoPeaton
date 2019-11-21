using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockingCrossingInfraction", menuName = "BlockingCrossing Infraction")]
public class InfractionBlockCrossing : InfractionSO
{
    /*Infraccion de estar parqueado donde no se debe, el controllador llama la regla que verifica si esta sobre un lugar
     * prohibido para vehiculos (Layer 9 "VehicleForbidenSpace") y si su velocidad es 0 (si esta quieto, esta parte falta
     * porque tengo que hablar como se esta manejando la aceleracion a ver como se evalua esta variable*/

    [SerializeField]
    private Vector2 boxCastSize;

    protected override bool RuleToCheck()
    {
        Debug.LogError("VOY A INTENTAR CHECKAR SI ESTA TAPANDO UNA CEBRA");
        RaycastHit2D hit;
        LayerMask mask = LayerMask.GetMask("CrossWalk");
        var rbObject = evaluatedObject.GetComponent<CarMovement>();

        hit = Physics2D.BoxCast(evaluatedObject.transform.position, boxCastSize, 0, Vector2.up, 1, mask);

        if (hit && rbObject.GetCurrentSpeed == 0)
        {
            CanvasManager._instance.GenerateFeedback("BlockCrossingReportCorrect");
            ScoreManager.instance.AddReport(true);
            CanvasManager._instance.ActivateCheckOrCross(true);
            Debug.LogError("3)Infraction Happened: "+"Infraction Name: " + this.name + " Evaluated Object: " + evaluatedObject.name);
            return false;
        }
        else
        {
            ScoreManager.instance.AddReport(false);
            CanvasManager._instance.ActivateCheckOrCross(false);
            CanvasManager._instance.GenerateFeedback("WrongReport");
            Debug.LogError("3) Infraction Didn't Happened: " + "Infraction Name: " + this.name + " Evaluated Object: " + evaluatedObject.name);
            return true;
        }
    }


}
