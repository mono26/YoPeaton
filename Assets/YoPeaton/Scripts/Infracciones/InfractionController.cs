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
    public void CheckAllInfractions(Collider2D _other)
    {
            ActivePedestrianCount = _other.gameObject.transform.root.GetChild(1).GetChild(0).GetComponent<HotZoneTrigger>().objectsInTriggerCount +
                                    _other.gameObject.transform.root.GetChild(1).GetChild(1).GetComponent<HotZoneTrigger>().objectsInTriggerCount;

            Debug.LogWarning("Active Pedestrian Count: " + ActivePedestrianCount);
            for (int i = 0; i < infractionsArray.Length; i++)
            {
                /*Debug.LogWarning("Entered First For Loop");
                Debug.LogWarning("i: " + i);
                Debug.LogWarning("Length of i: " + infractionsArray.Length);
                Debug.Log("Infractions Length: " + infractionsArray.Length);
                Debug.Log("Pedestrian HotZones: " + infractionsArray[i].currentPedestrianHotZones.Length);*/

                infractionsArray[i].currentPedestrianHotZones = new GameObject[2];
                if (ActivePedestrianCount > 0)
                {
                    infractionsArray[i].AreThereActivePedestrians = true;
                }
                else
                {
                    infractionsArray[i].AreThereActivePedestrians = false;
                }
                    for (int j = 0; j < _other.gameObject.transform.root.GetChild(2).childCount; j++)
                    {
                        /*Debug.LogWarning("Entered Second For Loop");
                        Debug.LogWarning("j: " + j);
                        Debug.LogWarning("Length of j: " + _other.gameObject.transform.root.GetChild(1).childCount);
                        Debug.LogWarning("Length of PedestrianHotZones: " + infractionsArray[i].currentPedestrianHotZones.Length);*/
                        infractionsArray[i].currentPedestrianHotZones[j] = _other.gameObject.transform.root.GetChild(1).GetChild(j).gameObject;
                        Debug.LogWarning("Hijo: " + infractionsArray[i].currentPedestrianHotZones[j].name);
                    }
                Debug.LogWarning("1) CheckAllInfractions Running (Evaluando el array de infracciones)");
                infractionsArray[i].evaluatedObject = controlledObject;
                infractionsArray[i].CheckForRuleViolation();
                if(infractionsArray[i].CheckForRuleViolation() == true)
                {
                    ScoreManager.instance.AddInfraction();
                }
             }


        /*for (int i = 0; i < infractionsArray.Length; i++)
        {

        }*/
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("HotZone"))
        {
            CheckAllInfractions(_other);
        }
        
    }


}
