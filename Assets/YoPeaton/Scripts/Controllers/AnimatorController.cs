using System.Collections;
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

    [SerializeField]
    private RuntimeAnimatorController[] animatorControllers;
   

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void SetAnimator(EntityTypes type)
    {
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/" + type);
    }
    public void SetCurrentAnimation(Vector3 direction)
    {
        //Debug.LogError(direction);
        //Debug.LogError(direction.x);
        //Debug.LogError(direction.y);
        if (direction == new Vector3(1.0f, 0f,0f))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
            return;
        } 
        
        else if (direction == new Vector3(-1.0f, 0f, 0f))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
            return;
        }

        if (direction == new Vector3(0f, 1f, 0f))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", true);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            return;
        }

        else if (direction == new Vector3(0f, -1f, 0f))
        {
            animator.SetBool("Front", true);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            return;
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
