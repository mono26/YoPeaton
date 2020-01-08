using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;        //Allows us to use Lists. 
using System;

public class SceneManagerTest : MonoBehaviour
{

    TutorialEventController tutorialController;
    public static SceneManagerTest instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private GameObject startCanvas;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case GameManager.victoryScene:
                startCanvas = null;
                loadingScreen = null;
                CanvasManager._instance.TurnOffHUD();
                if (GameManager.didPlayerLose == false)
                {
                    Debug.LogWarning("Cargue la scena de victoria");
                    ScoreManager.CalculateFinalScore();
                    CanvasManager._instance.FillTextArray();
                    CanvasManager._instance.FillVictoryButtons();
                    CanvasManager._instance.FillVictoryButtonMethods();
                    //CanvasManager._instance.CountToWhileWaiting();
                    //ScoreManager.instance.TestFillScoreArray();
                    //CanvasManager._instance.CountToScore();
                    CanvasManager._instance.StartCountSequence();
                    //StartCoroutine(BackToMenuCR());
                }
                else
                {
                    Debug.LogWarning("Cargue la scena de perder");
                    ScoreManager.CalculateLooserScore();
                    CanvasManager._instance.FillTextArray();
                    CanvasManager._instance.FillVictoryButtons();
                    CanvasManager._instance.FillVictoryButtonMethods();
                    //CanvasManager._instance.CountToWhileWaiting();
                    //ScoreManager.instance.TestFillScoreArray();
                    //CanvasManager._instance.CountToScore();
                    CanvasManager._instance.StartCountSequence();
                    //StartCoroutine(BackToMenuCR());
                }
                break;

            case GameManager.gameScene:
                startCanvas = null;
                loadingScreen = null;
                Debug.Log("SE DISPARO EL EVENTO DE QUE CARGO LA ESCENA DE TEST 2");
                ScoreManager.lifeTime = 200;
                CanvasManager._instance.TurnOnHUD();
                CanvasManager._instance.FillReferences();
                CanvasManager._instance.StartReplacementMethod();
                break;

            case GameManager.tutorialScene:
                startCanvas = null;
                loadingScreen = null;
                Debug.Log("SE DISPARO EL EVENTO DE QUE CARGO LA ESCENA DE TUTORIAL");
                //CanvasManager._instance.AssignDebugText();
                //CanvasManager._instance.debugText.text = "EVENTO DE SCENEMANAGER: CARGAR TUTORIAL";
                ScoreManager.lifeTime = 200;
                tutorialController = FindObjectOfType<TutorialEventController>();
                //CanvasManager._instance.debugText.text = "Tutorial Controller: " + tutorialController;
                tutorialController.FillReferences();
                //tutorialController.TurnOffSteps();
                CanvasManager._instance.TurnOnHUD();
                CanvasManager._instance.FillReferences();
                CanvasManager._instance.StartReplacementMethod();
                break;

            case GameManager.menuScene:
                startCanvas = GameObject.Find("StartCanvas");
                startCanvas.SetActive(true);
                loadingScreen = GameObject.Find("LoadingScreen");
                loadingScreen.SetActive(false);
                CanvasManager._instance.FillMenuBtns();
                CanvasManager._instance.TurnOffHUD();
                CanvasManager._instance.FillMenuBtnsMethods();
                break;
        }
    }

    IEnumerator BackToMenuCR()
    {
        yield return new WaitForSeconds(15);
        LoadScene(GameManager.menuScene);
    }

    public static void LoadNextScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadVictory()
    {
        SceneManager.LoadScene(GameManager.victoryScene);
        //CanvasManager._instance.StartSequence();
        //CanvasManager._instance.FillTextArray();
        //ScoreManager.instance.TestFillScoreArray();
        //StartCoroutine(WaitToCount());


    }

    public static void LoadGame(string scene)
    {
        instance.StartCoroutine("FakeLoad", scene);
    }

    public IEnumerator FakeLoad(string scene)
    {
        Debug.LogError("wtf");

        if (startCanvas != null)
        {
            startCanvas.SetActive(false);
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        yield return new WaitForSeconds(5.0f);
        StartCoroutine("LoadGameScene", scene);
    }

    public IEnumerator LoadGameScene(string scene)
    {
        AsyncOperation loadGameOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        yield return null;

        loadingScreen.SetActive(false);
    }

    public IEnumerator WaitToCount()
    {
        yield return new WaitForSeconds(1f);
        //CanvasManager._instance.CountToScore();
    }
    public static string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    
}