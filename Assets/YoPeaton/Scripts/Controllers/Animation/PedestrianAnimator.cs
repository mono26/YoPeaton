﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianAnimator : AnimatorController
{
    protected override void OnEnable() 
    {
        if(transitionsController != null)
        {
            transitionsController.onStartedAskingForCross += OnStartedToAskForPass;
        }
        if (entity)
        {
            entity.onEntityCollision += OnPedestrianRunOver;
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
        }
    }

    public override void OnMovement(OnEntityMovementEventArgs _args)
    {
        Debug.LogError(_args.MovementDirection);
        if (_args.MovementDirection.Equals(Vector3.right))
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

        else if (_args.MovementDirection.Equals(-Vector3.right))
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

        if (_args.MovementDirection.Equals(Vector3.up))
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

        else if (_args.MovementDirection.Equals(-Vector3.up))
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