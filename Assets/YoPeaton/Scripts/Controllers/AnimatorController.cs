using UnityEngine;
using System.Text.RegularExpressions;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private Animator directionalsAnimator;
    private IMovable movableComponent;
    private AITransitionsController transitionsController;
    [SerializeField]
    private EntityController entity;

    #region Unity calls
    private void Awake()
    {
        animator = GetComponent<Animator>();
        movableComponent = GetComponent<IMovable>();
        transitionsController = GetComponent<AITransitionsController>();
    }

    void Start()
    {
        string keyName = movableComponent.GetEntity.GetEntitySubType.ToString();
        animator.runtimeAnimatorController = AnimatorControllerDispatcher.GetInstance.Request(keyName);
    }

    void OnEnable()
    {
        if (movableComponent != null)
        {
            movableComponent.AddOnMovement(SetCurrentAnimation);
        }
        if(transitionsController != null)
            transitionsController.onStartedAskingForCross += OnStartedToAskForPass;
        if (entity)
        {
            entity.onStartChangingDirection += OnDirectionalStart;
            entity.onStopChangingDirection += OnDirectionalStop;
        }
    }

    void OnDisable()
    {
        if (movableComponent != null)
        {
            movableComponent.RemoveOnMovement(SetCurrentAnimation);
        }
        if (transitionsController != null)
            transitionsController.onStartedAskingForCross -= OnStartedToAskForPass;
        if (entity)
        {
            entity.onStartChangingDirection -= OnDirectionalStart;
            entity.onStopChangingDirection -= OnDirectionalStop;
        }
    }
    #endregion

    public void SetCurrentAnimation(Vector3 _direction)
    {
        MovementDirection direction = GetMovementDirection(_direction);
        if(movableComponent.GetEntity.GetEntityType == EntityType.Pedestrian)
        {
            if (direction.Equals(MovementDirection.Right))
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

            else if (direction.Equals(MovementDirection.Left))
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

            if (direction.Equals(MovementDirection.Up))
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

            else if (direction.Equals(MovementDirection.Down))
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
            // if (_direction.Equals(Vector3.right))
            // {
            //     animator.SetBool("Front", false);
            //     animator.SetBool("Back", false);
            //     animator.SetBool("Right", true);
            //     animator.SetBool("Left", false);
            //     animator.SetBool("WavingFront", false);
            //     animator.SetBool("WavingBack", false);
            //     animator.SetBool("WavingRight", false);
            //     animator.SetBool("WavingLeft", false);
            //     return;
            // }

            // else if (_direction.Equals(-Vector3.right))
            // {
            //     animator.SetBool("Front", false);
            //     animator.SetBool("Back", false);
            //     animator.SetBool("Right", false);
            //     animator.SetBool("Left", true);
            //     animator.SetBool("WavingFront", false);
            //     animator.SetBool("WavingBack", false);
            //     animator.SetBool("WavingRight", false);
            //     animator.SetBool("WavingLeft", false);
            //     return;
            // }

            // if (_direction.Equals(Vector3.up))
            // {
            //     animator.SetBool("Front", false);
            //     animator.SetBool("Back", true);
            //     animator.SetBool("Right", false);
            //     animator.SetBool("Left", false);
            //     animator.SetBool("WavingFront", false);
            //     animator.SetBool("WavingBack", false);
            //     animator.SetBool("WavingRight", false);
            //     animator.SetBool("WavingLeft", false);
            //     return;
            // }

            // else if (_direction.Equals(-Vector3.up))
            // {
            //     animator.SetBool("Front", true);
            //     animator.SetBool("Back", false);
            //     animator.SetBool("Right", false);
            //     animator.SetBool("Left", false);
            //     animator.SetBool("WavingFront", false);
            //     animator.SetBool("WavingBack", false);
            //     animator.SetBool("WavingRight", false);
            //     animator.SetBool("WavingLeft", false);
            //     return;
            // }
        }

        if (movableComponent.GetEntity.GetEntityType == EntityType.Car)
        {
            if (_direction.Equals(Vector3.right))
            {
                animator.SetBool("Front", false);
                animator.SetBool("Back", false);
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                return;
            }

            else if (_direction.Equals(-Vector3.right))
            {
                animator.SetBool("Front", false);
                animator.SetBool("Back", false);
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                return;
            }

            if (_direction.Equals(Vector3.up))
            {
                animator.SetBool("Front", false);
                animator.SetBool("Back", true);
                animator.SetBool("Right", false);
                animator.SetBool("Left", false);
                return;
            }

            else if (_direction.Equals(-Vector3.up))
            {
                animator.SetBool("Front", true);
                animator.SetBool("Back", false);
                animator.SetBool("Right", false);
                animator.SetBool("Left", false);
                return;
            }
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

    public void OnPedestrianRunOver()
    {

        if (movableComponent.GetEntity.GetEntityType == EntityType.Pedestrian)
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
            return;
        }
    }
    public void SetCurrentAnimation2D(Vector2 direction)
    {
        //Debug.LogError(direction);
        //Debug.LogError(direction.x);
        //Debug.LogError(direction.y);
        if (direction == Vector2.right)
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
            return;
        }

        else if (direction == Vector2.left)
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
            return;
        }

        if (direction == Vector2.down)
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", true);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            return;
        }

        else if (direction == Vector2.up)
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

    private MovementDirection GetMovementDirection(Vector3 _direction)
    {
        MovementDirection direction = MovementDirection.Down;
        if (_direction.x >  _direction.y)
        {
            if (_direction.x < 0)
            {
                direction = MovementDirection.Left;
            }
            else
            {
                direction = MovementDirection.Right;
            }
        }
        else
        {
            if (_direction.y < 0)
            {
                direction = MovementDirection.Down;
            }
            else
            {
                direction = MovementDirection.Up;
            }
        }
        return direction;
    }

    public void OnStartedToAskForPass(Vector3 direction) 
    {
        if(movableComponent.GetEntity.GetEntityType == EntityType.Pedestrian)
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

    private void OnDirectionalStart(Vector3 _directional)
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

public enum MovementDirection
{
    Right, Left, Up, Down
}