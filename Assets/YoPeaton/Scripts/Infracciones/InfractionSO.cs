using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InfractionSO : ScriptableObject
{
    [SerializeField]
    protected string InfractionName;

    public GameObject evaluatedObject;

    public GameObject[]  currentPedestrianHotZones;


    public bool AreThereActivePedestrians;
    /*NOTA DE FIN DE DIA: Hay que usar esta variable para recibir la cantidad de peatones o vehiculos dentro del crosswalk actual, 
     * y luego pasarle esa variable a cada uno de las infracciones que se van a evaluar, así se pueden realizar las otras reglas 
     * (cuando se toque el vehiculo, el infraction controller pide la cantidad de peatones dentro del trigger de hotzone al HotZoneTrigger
     * Script, y de ahi le dice a cada infraccion si hay o no hay poeatones activos en este trigger para que ya cada regla pueda usar el dato
     * para determinar si se rompio o no*/


    public bool CheckForRuleViolation()
    {
        Debug.LogWarning("2) CheckForRuleViolation Running En Infracción: " + InfractionName);
        //bool isRuleBroken = RuleToCheck();
        if (RuleToCheck())
        {
            Debug.LogError(this.name + ": Broken Rule");
            return true;
        }
        else
        {
            Debug.LogError(this.name + ": No Rule Broken");
            return false;
        }
    }

    protected virtual bool RuleToCheck()
    {
        return false;
    }

    /*protected virtual bool RuleToCheck(GameObject evaluatedObject)
    {
        return false;
    }*/
}
