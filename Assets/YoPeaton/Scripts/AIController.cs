using UnityEngine;

public class AIController : EntityController
{
    protected override bool ShouldStop() {
        // Raycast for a vehicle up front.
        // Raycast for pedestrians.
        return false;
    }
}
