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
}
