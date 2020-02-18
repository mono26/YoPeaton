using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialEventController : MonoBehaviour
{
    public GameObject tutorialSteps;
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


    public void FillReferences()
    {
        Debug.Log("voy a llenar las referencias de los tutorial steps");
        tutorialSteps = GameObject.Find("TutorialStepsParent");
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
        tutorialStep0.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep1.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep2.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep3.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep4.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep5.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep6.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep7.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep8.transform.GetChild(0).gameObject.SetActive(false);
        tutorialSteps.GetComponent<Canvas>().enabled = false;
        /*tutorialStep0.gameObject.SetActive(false);
        tutorialStep1.gameObject.SetActive(false);
        tutorialStep2.gameObject.SetActive(false);
        tutorialStep3.gameObject.SetActive(false);
        tutorialStep4.gameObject.SetActive(false);
        tutorialStep5.gameObject.SetActive(false);
        tutorialStep6.gameObject.SetActive(false);
        tutorialStep7.gameObject.SetActive(false);
        tutorialStep8.gameObject.SetActive(false);*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "TutorialTrigger (0)")
        {
            print("COLISION CON TUTORIAL TRIGGER 0");
            currentStep = tutorialStep0;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep0.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep0.gameObject.SetActive(true);
            Time.timeScale = 0;
            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));

        }
        if (collision.gameObject.name == "TutorialTrigger (1)")
        {
            print("COLISION CON TUTORIAL TRIGGER 1");
            currentStep = tutorialStep1;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep1.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep1.gameObject.SetActive(true);
            Time.timeScale = 0;

            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (2)")
        {
            print("COLISION CON TUTORIAL TRIGGER 2");
            currentStep = tutorialStep2;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep2.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep2.gameObject.SetActive(true);
            Time.timeScale = 0;
            //tutorialSteps.GetComponent<Canvas>().sortingOrder = 1;

            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (3)")
        {
            print("COLISION CON TUTORIAL TRIGGER 3");
            currentStep = tutorialStep3;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep3.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep3.gameObject.SetActive(true);
            Time.timeScale = 0;

            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (4)")
        {
            print("COLISION CON TUTORIAL TRIGGER 4");
            currentStep = tutorialStep4;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep4.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep4.gameObject.SetActive(true);
            Time.timeScale = 0;
            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (5)")
        {
            print("COLISION CON TUTORIAL TRIGGER 5");
            currentStep = tutorialStep5;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep5.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep5.gameObject.SetActive(true);
            Time.timeScale = 0;
            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (6)")
        {
            print("COLISION CON TUTORIAL TRIGGER 6");
            currentStep = tutorialStep6;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep6.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep6.gameObject.SetActive(true);
            Time.timeScale = 0;
            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (7)")
        {
            print("COLISION CON TUTORIAL TRIGGER 7");
            currentStep = tutorialStep7;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep7.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep7.gameObject.SetActive(true);
            Time.timeScale = 0;
            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }
        if (collision.gameObject.name == "TutorialTrigger (8)")
        {
            print("COLISION CON TUTORIAL TRIGGER 8");
            currentStep = tutorialStep8;
            tutorialSteps.GetComponent<Canvas>().enabled = true;
            tutorialStep8.transform.GetChild(0).gameObject.SetActive(true);
            //tutorialStep8.gameObject.SetActive(true);
            Time.timeScale = 0;
            //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 0;
            //StartCoroutine(TurnOffCurrentStepCR(currentStep));
        }

    }

    IEnumerator TurnOffCurrentStepCR(GameObject currentStep)
    {
        yield return new WaitForSecondsRealtime(3);
        currentStep.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void TurnOffCurrentStepMehtod(GameObject PcurrentStep)
    {
        //tutorialSteps.GetComponent<Canvas>().sortingOrder = 0;
        //CanvasManager._instance.wholeCanvas.GetComponent<Canvas>().sortingOrder = 1;
        PcurrentStep.transform.GetChild(0).gameObject.SetActive(false);
        tutorialSteps.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
        currentStep = null; 
    }

    public void TurnOffStepBtn(GameObject objectToTurnOff)
    {
        objectToTurnOff.SetActive(false);
        Time.timeScale = 1;
        Debug.LogError("ME HUNDIERON");
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
