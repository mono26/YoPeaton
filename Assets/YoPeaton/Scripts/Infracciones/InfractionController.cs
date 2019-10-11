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
    // Start is called before the first frame update
    void Update()
    {
        Debug.LogError(controlledObject.GetComponent<Rigidbody2D>().velocity);
    }
    public void CheckAllInfractions()
    {
        Debug.LogWarning("1) CheckAllInfractions Running (Evaluando el array de infracciones)");
        for (int i = 0; i < infractionsArray.Length; i++)
        {
            infractionsArray[i].evaluatedObject = controlledObject;
            infractionsArray[i].CheckForRuleViolation();
        }
    }
}
