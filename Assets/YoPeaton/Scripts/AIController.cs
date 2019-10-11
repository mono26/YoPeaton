using UnityEngine;

public class AIController : EntityController
{
    float distanceTravelled;
    private float timeOnCurrentPath;
    private bool move = true;


    protected override bool ShouldStop() {
        // Raycast for a vehicle up front.
        // Raycast for pedestrians.
        return false;
    }
}
