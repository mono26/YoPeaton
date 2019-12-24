using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianAnimator : EntityAnimationController
{

    public Vector3 currentDirection;
    protected override void OnEnable() 
    {
        if(transitionsController != null)
        {
            transitionsController.onStartedAskingForCross += OnStartedToAskForPass;
        }
        if (entity)
        {
            entity.onEntityCollision += OnPedestrianRunOver;
            entity.GetMovableComponent.AddOnMovement(OnMovement);
        }
    }

    protected override void OnDisable()
    {
        if (transitionsController != null)
        {
            transitionsController.onStartedAskingForCross -= OnStartedToAskForPass;
        }
        if (entity)
        {
            entity.onEntityCollision -= OnPedestrianRunOver;
            entity.GetMovableComponent.RemoveOnMovement(OnMovement);
        }
    }

    public override void OnMovement(OnEntityMovementEventArgs _args)
    {
        currentDirection = _args.MovementDirection;
        //Debug.LogError(_args.MovementDirection);
        if (_args.MovementDirection == new Vector3(1, 0, 0))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
            animator.SetBool("WavingFront", false);
            animator.SetBool("WavingBack", false);
            animator.SetBool("WavingRight", false);
            animator.SetBool("WavingLeft", false);
            return;
        }

        else if (_args.MovementDirection == new Vector3(-1, 0, 0))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
            animator.SetBool("WavingFront", false);
            animator.SetBool("WavingBack", false);
            animator.SetBool("WavingRight", false);
            animator.SetBool("WavingLeft", false);
            return;
        }

        if (_args.MovementDirection == new Vector3(0, 1, 0))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", true);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("WavingFront", false);
            animator.SetBool("WavingBack", false);
            animator.SetBool("WavingRight", false);
            animator.SetBool("WavingLeft", false);
            return;
        }

        else if (_args.MovementDirection == new Vector3(0, -1, 0))
        {
            animator.SetBool("Front", true);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("WavingFront", false);
            animator.SetBool("WavingBack", false);
            animator.SetBool("WavingRight", false);
            animator.SetBool("WavingLeft", false);
            return;
        }
    }

    private void OnPedestrianRunOver()
    {
        print("ON PEDESTRIAN RUN OVER METHOD CALLED");
        animator.SetBool("Front", false);
        animator.SetBool("Back", false);
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("WavingFront", false);
        animator.SetBool("WavingBack", false);
        animator.SetBool("WavingRight", false);
        animator.SetBool("WavingLeft", false);
        animator.SetBool("RunOver", true);
    }

    public void OnPublicPedestrianRunOver()
    {
        print("ON PEDESTRIAN RUN OVER METHOD CALLED");
        animator.SetBool("Front", false);
        animator.SetBool("Back", false);
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("WavingFront", false);
        animator.SetBool("WavingBack", false);
        animator.SetBool("WavingRight", false);
        animator.SetBool("WavingLeft", false);
        animator.SetBool("RunOver", true);
    }

    public void OnStartedToAskForPass(Vector3 direction) 
    {
        if (direction == new Vector3(1.0f, 0f, 0f))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("WavingFront", false);
            animator.SetBool("WavingBack", false);
            animator.SetBool("WavingRight", true);
            animator.SetBool("WavingLeft", false);
            return;
        }

        else if (direction == new Vector3(-1.0f, 0f, 0f))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("WavingFront", false);
            animator.SetBool("WavingBack", false);
            animator.SetBool("WavingRight", false);
            animator.SetBool("WavingLeft", true);
            return;
        }

        if (direction == new Vector3(0f, 1f, 0f))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("WavingFront", false);
            animator.SetBool("WavingBack", true);
            animator.SetBool("WavingRight", false);
            animator.SetBool("WavingLeft", false);
            return;
        }

        else if (direction == new Vector3(0f, -1f, 0f))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("WavingFront", true);
            animator.SetBool("WavingBack", false);
            animator.SetBool("WavingRight", false);
            animator.SetBool("WavingLeft", false);
            return;
        }
    }
}
