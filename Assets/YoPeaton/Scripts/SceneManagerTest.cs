﻿using UnityEngine;
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

    public void LoadNextScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}