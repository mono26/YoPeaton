﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    public Animator Animator
    {
        get
        {
            return animator;
        }
        set
        {
            animator = value;
        }
    }

   

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void SetAnimator(string id)
    {
        Debug.Log(id);
        Debug.Log("Hi");
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/YoPeaton/AnimatorControllers/Female");
        /*  switch (id)
          {
              case "Male":
                  animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/YoPeaton/AnimatorControllers/Male");
                  break;

          }*/

    }
    public void SetCurrentAnimation(Vector3 direction)
    {
        //Debug.LogError(direction);
        //Debug.LogError(direction.x);
        //Debug.LogError(direction.y);
        if (direction.x >= 0f && direction.x<=1f)
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
        } 
        
        else if (direction.x < 0f && direction.x >= -1.0f)
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
        }

        if (direction.y >= 0f && direction.y <= 1.0f)
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", true);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
        }

        else if (direction.y >= 0f && direction.y >= -1.0f)
        {
            animator.SetBool("Front", true);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
        }
       

        //switch (direction)
        //{
        //    case Directions.North:
        //        animator.SetBool("Front", true);
        //        animator.SetBool("Back", false);
        //        animator.SetBool("Right", false);
        //        animator.SetBool("Left", false);
        //        break;

        //    case Directions.South:
        //        animator.SetBool("Front", false);
        //        animator.SetBool("Back", true);
        //        animator.SetBool("Right", false);
        //        animator.SetBool("Left", false);
        //        break;

        //    case Directions.East:
        //        animator.SetBool("Front", false);
        //        animator.SetBool("Back", false);
        //        animator.SetBool("Right", true);
        //        animator.SetBool("Left", false);
        //        break;

        //    case Directions.West:
        //        animator.SetBool("Front", false);
        //        animator.SetBool("Back", false);
        //        animator.SetBool("Right", false);
        //        animator.SetBool("Left", true);
        //        break;
        //}
    }

}
