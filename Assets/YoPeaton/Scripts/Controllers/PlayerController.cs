using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{

    public static PlayerController instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.

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

        if (!input)
        {
            input = GetComponent<PlayerCarInput>();
        }
    }
    public float lifeTime = 3000f;
    [SerializeField]
    private PlayerCarInput input = null;

    /*protected void Awake() {

    }*/

    private void Update()
    {
        lifeTime--;
    }
    protected override bool ShouldStop() {
        return input.IsBraking;
    }

    protected override bool ShouldSlowDown() {
        return input.IsBraking;
    }
}
