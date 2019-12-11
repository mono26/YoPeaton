using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventController : MonoBehaviour
{
    private GameObject tutorialSteps;
    public GameObject tutorialStep0;
    public GameObject tutorialStep1;
    public GameObject tutorialStep2;
    public GameObject tutorialStep3;
    public GameObject tutorialStep4;
    public GameObject tutorialStep5;
    public GameObject tutorialStep6;
    public GameObject tutorialStep7;
    public GameObject tutorialStep8;

    public GameObject currentStep;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FillReferences()
    {
        Debug.Log("voy a llenar las referencias de los tutorial steps");
        //CanvasManager._instance.debugText.text = "EVENTO DE tutorial Controller: Llenar referencias";
        /*tutorialStep0 = GameObject.Find("TutorialSteps (0)");
        tutorialStep1 = GameObject.Find("TutorialSteps (1)");
        tutorialStep2 = GameObject.Find("TutorialSteps (2)");
        tutorialStep3 = GameObject.Find("TutorialSteps (3)");
        tutorialStep4 = GameObject.Find("TutorialSteps (4)");
        tutorialStep5 = GameObject.Find("TutorialSteps (5)");
        tutorialStep6 = GameObject.Find("TutorialSteps (6)");
        tutorialStep7 = GameObject.Find("TutorialSteps (7)");
        tutorialStep8 = GameObject.Find("TutorialSteps (8)");*/
        tutorialSteps = GameObject.Find("TutorialStepsParent");
        if(tutorialSteps != null)
            //CanvasManager._instance.debugText.text = "Tutorial Steps: " + tutorialSteps.name;
        tutorialStep0 = tutorialSteps.transform.GetChild(0).gameObject;
        tutorialStep1 = tutorialSteps.transform.GetChild(1).gameObject;
        tutorialStep2 = tutorialSteps.transform.GetChild(2).gameObject;
        tutorialStep3 = tutorialSteps.transform.GetChild(3).gameObject;
        tutorialStep4 = tutorialSteps.transform.GetChild(4).gameObject;
        tutorialStep5 = tutorialSteps.transform.GetChild(5).gameObject;
        tutorialStep6 = tutorialSteps.transform.GetChild(6).gameObject;
        tutorialStep7 = tutorialSteps.transform.GetChild(7).gameObject;
        tutorialStep8 = tutorialSteps.transform.GetChild(8).gameObject;
        TurnOffSteps();
    }

    public void TurnOffSteps()
    {
        print("Apagando los steps");
        //CanvasManager._instance.debugText.text = "EVENTO DE tutorial Controller: Apagar Steps Metodo";
        //StartCoroutine(TurnOffAllStepsCR());
        tutorialStep0.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep1.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep2.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep3.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep4.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep5.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep6.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep7.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep8.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "TutorialTrigger (0)")
        {
            print("COLISION CON TUTORIAL TRIGGER 0");
            currentStep = tutorialStep0;
            tutorialStep0.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));

        }
        if (collision.gameObject.name == "TutorialTrigger (1)")
        {
            print("COLISION CON TUTORIAL TRIGGER 1");
            currentStep = tutorialStep1;
            tutorialStep1.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (2)")
        {
            print("COLISION CON TUTORIAL TRIGGER 2");
            currentStep = tutorialStep2;
            tutorialStep2.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (3)")
        {
            print("COLISION CON TUTORIAL TRIGGER 3");
            currentStep = tutorialStep3;
            tutorialStep3.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (4)")
        {
            print("COLISION CON TUTORIAL TRIGGER 4");
            currentStep = tutorialStep4;
            tutorialStep4.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (5)")
        {
            print("COLISION CON TUTORIAL TRIGGER 5");
            currentStep = tutorialStep5;
            tutorialStep5.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (6)")
        {
            print("COLISION CON TUTORIAL TRIGGER 6");
            currentStep = tutorialStep6;
            tutorialStep6.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (7)")
        {
            print("COLISION CON TUTORIAL TRIGGER 7");
            currentStep = tutorialStep7;
            tutorialStep7.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (8)")
        {
            print("COLISION CON TUTORIAL TRIGGER 8");
            currentStep = tutorialStep8;
            tutorialStep8.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }

    }

    IEnumerator TurnOffCurrentStepCR(GameObject currentStep)
    {
        yield return new WaitForSecondsRealtime(3);
        currentStep.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    IEnumerator TurnOffAllStepsCR()
    {
        yield return new WaitForEndOfFrame();
        //CanvasManager._instance.debugText.text = "EVENTO DE tutorial Controller: Apagar Steps Corutina";
        tutorialStep0.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep1.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep2.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep3.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep4.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep5.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep6.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep7.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep8.transform.GetChild(0).gameObject.SetActive(false);
    }
}
