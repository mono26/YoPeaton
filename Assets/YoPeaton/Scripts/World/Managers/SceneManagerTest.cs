using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;        //Allows us to use Lists. 

public class SceneManagerTest : MonoBehaviour
{

    public static SceneManagerTest instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

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
        switch (scene.name.ToLower())
        {
            case "victoryscreenscene":
                Debug.LogError("Cargue la scena de victoria");
                CanvasManager._instance.FillTextArray();
                //CanvasManager._instance.CountToWhileWaiting();
                //ScoreManager.instance.TestFillScoreArray();
                //CanvasManager._instance.CountToScore();
                CanvasManager._instance.StartCountSequence();
                break;
        }
    }
    public static void LoadNextScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);

    }

    public void LoadVictory()
    {
        SceneManager.LoadScene("VictoryScreenScene");
        //CanvasManager._instance.StartSequence();
        //CanvasManager._instance.FillTextArray();
        //ScoreManager.instance.TestFillScoreArray();
        //StartCoroutine(WaitToCount());


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