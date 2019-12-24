using UnityEngine;

public class CarAnimator : EntityAnimationController
{
    [SerializeField]
    private Animator directionalsAnimator;

    protected override void OnEnable() 
    {
        if (entity)
        {
            entity.GetDirectionChangeComponent.onStartDirectionChange += OnStartDirectionChange;
            entity.GetDirectionChangeComponent.onStopChangingDirection += OnDirectionalStop;
            entity.GetMovableComponent.AddOnMovement(OnMovement);
        }

    }

    protected override void OnDisable() 
    {
        if (entity)
        {
            entity.GetDirectionChangeComponent.onStartDirectionChange -= OnStartDirectionChange;
            entity.GetDirectionChangeComponent.onStopChangingDirection -= OnDirectionalStop;
            entity.GetMovableComponent.RemoveOnMovement(OnMovement);
        }

    }


    public override void OnMovement(OnEntityMovementEventArgs _args)
    {
        //Debug.LogError(_args.MovementDirection);
        if (_args.MovementDirection == new Vector3(1, 0, 0))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
            return;
        }

        else if (_args.MovementDirection == new Vector3(-1,0,0))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
            return;
        }

        if (_args.MovementDirection == new Vector3(0, 1, 0))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", true);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            return;
        }

        else if (_args.MovementDirection == new Vector3(0, -1, 0))
        {
            animator.SetBool("Front", true);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            return;
        }
    }

    private void OnStartDirectionChange(OnStartDirectionChangeArgs _args)
    {
        StartDirectional(_args.NextDirection);
    }

    private void StartDirectional(Vector3 _directional)
    {
        if (directionalsAnimator)
        {
            //Right
            if (_directional.Equals(Vector3.right))
            {
                directionalsAnimator.SetBool("Left", false);
                directionalsAnimator.SetBool("Right", true);
            }
            //Left
            else if (_directional.Equals(-Vector3.right))
            {
                directionalsAnimator.SetBool("Left", true);
                directionalsAnimator.SetBool("Right", false);
            }
        }
    }

    private void OnDirectionalStop()
    {
        if (directionalsAnimator)
        {
            directionalsAnimator.SetBool("Left", false);
            directionalsAnimator.SetBool("Right", false);
        }
    }
}
