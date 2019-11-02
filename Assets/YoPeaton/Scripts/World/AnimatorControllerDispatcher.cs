using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO refactor a Dictionary y custom inspector.
[System.Serializable]
public struct EntityAnimatorControllerPair {
    public EntityTypes entity;
    public RuntimeAnimatorController controller;
}

public class AnimatorControllerDispatcher : Singleton<AnimatorControllerDispatcher>
{
    [SerializeField]
    private List<EntityAnimatorControllerPair> controllerPairs = new List<EntityAnimatorControllerPair>();

    protected override void Awake() {
        base.Awake();
    }

    public RuntimeAnimatorController Request(string _nameKey) {
        RuntimeAnimatorController controllerToReturn = null;
        EntityTypes requestedType = (EntityTypes)System.Enum.Parse(typeof(EntityTypes),  _nameKey);
        if (controllerPairs.Count > 0) {
            for (int i = 0; i < controllerPairs.Count; i++) {
                if (controllerPairs[i].entity.Equals(requestedType)) {
                    controllerToReturn = controllerPairs[i].controller;
                }
            }
        }
        else {
            DebugController.LogErrorMessage("There is no entity-animator controllers pair.");
        }
        return controllerToReturn;
    }
}
