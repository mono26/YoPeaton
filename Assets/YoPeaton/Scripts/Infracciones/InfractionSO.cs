using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InfractionSO : ScriptableObject
{
    [SerializeField]
    protected string InfractionName;

    public GameObject evaluatedObject;



    public bool CheckForRuleViolation()
    {
        Debug.LogWarning("2) CheckForRuleViolation Running En Infracción: " + InfractionName);
        bool isRuleBroken = RuleToCheck();
        if (isRuleBroken)
        {
            Debug.LogError(this.name + "Broken Rule");
            return true;
        }
        else
        {
            Debug.LogError(this.name + "No Rule Broken");
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
