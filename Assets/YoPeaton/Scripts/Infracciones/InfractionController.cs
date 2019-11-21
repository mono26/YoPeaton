using System;
using UnityEngine;

public class InfractionController : MonoBehaviour
{
    /*El controlador recibe scriptable objects y cuando se toca un vehiculo o peaton, corre el metodo de CheckAllInfractions,
     * para verificar si esta incumpliendo alguna infraccion y si encuentra alguna que este infringiendo, le informara al jugador
     * a traves de un mensaje en su interfaz*/

    [SerializeField]
    private InfractionSO[] infractionsArray;

    [SerializeField]
    private GameObject controlledObject;

    [SerializeField]
    private int ActivePedestrianCount;

    // Start is called before the first frame update
    private void Start()
    {
        controlledObject = this.gameObject;

    }

    public void CheckAllInfractions(Crosswalk _crosswalk)
    {
        if (_crosswalk != null)
        {
            ActivePedestrianCount = _crosswalk.GetNumberOfCrossingPedestrians;
            DebugController.LogWarningMessage("Active Pedestrian Count: " + ActivePedestrianCount);
            DebugController.LogErrorMessage("Checking infractions");
            for (int i = 0; i < infractionsArray.Length; i++)
            {
                if (ActivePedestrianCount > 0)
                {
                    infractionsArray[i].AreThereCrossingPedestrians = true;
                }
                else
                {
                    infractionsArray[i].AreThereCrossingPedestrians = false;
                }

                Debug.LogWarning("1) CheckAllInfractions Running (Evaluando el array de infracciones)");
                infractionsArray[i].evaluatedObject = controlledObject;
                infractionsArray[i].CheckForRuleViolation();
                if (infractionsArray[i].CheckForRuleViolation() == true)
                {
                    ScoreManager.instance.AddInfraction();
                }
            }
        }
        else
        {
            for (int i = 0; i < infractionsArray.Length; i++)
            {
                Debug.LogError("ESTOY EVALUANDO UNA INFRACCION SIN TENER CROSSWALK CERQUITA");
                infractionsArray[i].AreThereCrossingPedestrians = false;
                Debug.LogWarning("1) CheckAllInfractions Running (Evaluando el array de infracciones)");
                infractionsArray[i].evaluatedObject = controlledObject;
                infractionsArray[i].CheckForRuleViolation();
                if (infractionsArray[i].CheckForRuleViolation() == true)
                {
                    ScoreManager.instance.AddInfraction();
                }
            }


            /*for (int i = 0; i < infractionsArray.Length; i++)
            {

            }*/
        }

        // private void OnTriggerEnter2D(Collider2D _other) {
        //     if (_other.CompareTag("CrossWalk")) {
        //         CheckAllInfractions(_other.GetComponent<Crosswalk>());
        //     }

        // }
    }
}
