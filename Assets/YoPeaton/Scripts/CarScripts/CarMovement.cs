using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D carBody = null;
    [SerializeField]
    private float maxSpeed = 0.0f;
    [SerializeField]
    private float acceleration = 0.0f;
    public bool testAccel = true;
    private void Update()
    {
        if (testAccel)
        {
            this.gameObject.transform.Translate(Vector3.right * Time.deltaTime);
        }
        if(!testAccel)
        {
            this.gameObject.transform.Translate(Vector3.zero);
        }
    }
}
