using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{

    //Awake is always called before any Start functions
    void Awake()
    {

        if (!input)
        {
            input = GetComponent<PlayerCarInput>();
        }
    }
    public static float lifeTime = 500f;
    [SerializeField]
    private PlayerCarInput input = null;

    /*protected void Awake() {

    }*/

        //MACHETAZO PARA EL PUNTAJE//
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
