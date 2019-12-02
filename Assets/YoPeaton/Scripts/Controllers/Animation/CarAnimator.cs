using UnityEngine;

public class CarAnimator : AnimatorController
{
    [SerializeField]
    private Animator directionalsAnimator;

    protected override void OnEnable() 
    {
        if (entity)
        {
            entity.onStartDirectionChange += OnStartDirectionChange;
            entity.onStopChangingDirection += OnDirectionalStop;
        }
    }

    protected override void OnDisable() 
    {
        if (entity)
        {
            entity.onStartDirectionChange -= OnStartDirectionChange;
            entity.onStopChangingDirection -= OnDirectionalStop;
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
            return;
        }

        else if (_args.MovementDirection.Equals(-Vector3.right))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
            return;
        }

        if (_args.MovementDirection.Equals(Vector3.up))
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", true);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            return;
        }

        else if (_args.MovementDirection.Equals(-Vector3.up))
        {
            animator.SetBool("Front", true);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            return;
        }
    }

    private void OnStartDirectionChange(OnEntityStartDirectionChangeArgs _args)
    {
        StartDirectional(_args.Direction);
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
