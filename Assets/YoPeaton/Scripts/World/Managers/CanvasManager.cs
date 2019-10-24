using UnityEngine.UI;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager _instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.



    [SerializeField]
    private Canvas baseCanvas;
    [SerializeField]
    private Canvas pausaCanvas;
    [SerializeField]
    public Canvas optInButtonCanvas;
    [SerializeField]
    public Canvas identifyCrossingCanvas;

    private string currentActiveCanvas;
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (_instance == null)

            //if not, set instance to this
            _instance = this;

        //If instance already exists and it's not this:
        else if (_instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Desastivar todos los otros canvas
        pausaCanvas.enabled = false;
        optInButtonCanvas.enabled = false;
        identifyCrossingCanvas.enabled = false;

        //Activar unicamente el base
        baseCanvas.enabled = true;
        currentActiveCanvas = baseCanvas.name;
    }

    private void Update()
    {
        Debug.LogError("Timescale: " + Time.timeScale);
    }
    //MANEJO DE CANVAS//
    public void ActivatePauseCanvas()
    {
        if (GameManager.isPaused)
        {
            baseCanvas.enabled = true;
            pausaCanvas.enabled = false;
        }
        else 
        {
            baseCanvas.enabled = false;
            pausaCanvas.enabled = true;
        }
    }

    public void ActivateSpecificCanvas(string canvasToActivate)
    {
        if (canvasToActivate == "OptInCanvas")
        {
            optInButtonCanvas.enabled = true;
        }
        if (canvasToActivate == "SignalIdentificationCanvas")
        {
              identifyCrossingCanvas.enabled = true;
              optInButtonCanvas.enabled = false;
        }
    }

    //BOTONES//
    public void PressPauseBtn()
    {
        Debug.LogWarning("Press Pause Button");
        GameManager.PauseGame();
        ActivatePauseCanvas();
    }

    public void BackToBaseCanvas()
    {
        baseCanvas.enabled = true;
    }
}
