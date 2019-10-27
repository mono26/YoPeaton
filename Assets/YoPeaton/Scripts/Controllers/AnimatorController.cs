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

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void SetCurrentAnimation(Directions direction)
    {
        switch (direction)
        {
            case Directions.North:
                animator.SetBool("Front", true);
                break;

            case Directions.South:
                animator.SetBool("Back", true);
                break;

            case Directions.East:
                animator.SetBool("Right", true);
                break;

            case Directions.West:
                animator.SetBool("Left", true);
                break;
        }
    }

}
