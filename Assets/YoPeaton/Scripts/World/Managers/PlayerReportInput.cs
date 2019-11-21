using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerReportInput : MonoBehaviour
{
    public Camera cam;
    [SerializeField]
    private AIController controller;

    private void Start()
    {

    }
    void Update()
    {

        if (Input.touchCount > 0)
        {
            Vector3 mousePos = Input.GetTouch(0).position;
            mousePos.z = 10;
            LayerMask mask = LayerMask.GetMask("Car");
            Vector3 screenPos = cam.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity, mask);

            print(hit.collider);
            if (hit)
            {
                if (hit.collider.tag == "Car" && hit.collider.name != "PlayerCar_PFB")
                {

                    controller = hit.collider.gameObject.GetComponent<AIController>();
                    print(hit.collider.name);
                    controller.CheckIfIsBreakingTheLaw();

                }
            }
        }

        //TEST FOR MOUSE IN EDITOR//
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            LayerMask mask = LayerMask.GetMask("Car");
            //Debug.LogError("Position: " + mousePos);
            Vector3 screenPos = cam.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity, mask);

            //print(hit.collider);


            if (hit)
            {
                if (hit.collider.tag == "Car" && hit.collider.name != "PlayerCar_PFB")
                {
                    Debug.LogError("Collider: " + hit.collider.name);
                    controller = hit.collider.gameObject.GetComponent<AIController>();
                    //print(hit.collider.name);
                    controller.CheckIfIsBreakingTheLaw();

                }
            }
        }
    }

    IEnumerator StartCheck()
    {

        yield return null;
    }

}
