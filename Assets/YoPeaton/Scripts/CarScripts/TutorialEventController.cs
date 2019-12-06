using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventController : MonoBehaviour
{
    public GameObject tutorialStep0;
    public GameObject tutorialStep1;
    public GameObject tutorialStep2;
    public GameObject tutorialStep3;
    public GameObject tutorialStep4;
    public GameObject tutorialStep5;
    public GameObject tutorialStep6;
    public GameObject tutorialStep7;

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
        tutorialStep0 = GameObject.Find("TutorialSteps (0)");
        tutorialStep1 = GameObject.Find("TutorialSteps (1)");
        tutorialStep2 = GameObject.Find("TutorialSteps (2)");
        /*tutorialStep3 = GameObject.Find("TutorialSteps (3)");
        tutorialStep4 = GameObject.Find("TutorialSteps (4)");
        tutorialStep5 = GameObject.Find("TutorialSteps (5)");
        tutorialStep6 = GameObject.Find("TutorialSteps (6)");
        tutorialStep7 = GameObject.Find("TutorialSteps (7)");*/

        tutorialStep0.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep1.transform.GetChild(0).gameObject.SetActive(false);
        tutorialStep2.transform.GetChild(0).gameObject.SetActive(false);
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

        }
        if (collision.gameObject.name == "TutorialTrigger (4)")
        {

        }
        if (collision.gameObject.name == "TutorialTrigger (5)")
        {

        }
        if (collision.gameObject.name == "TutorialTrigger (6)")
        {

        }
        if (collision.gameObject.name == "TutorialTrigger (7)")
        {

        }

    }

    IEnumerator TurnOffCurrentStepCR(GameObject currentStep)
    {
        yield return new WaitForSecondsRealtime(3);
        currentStep.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
