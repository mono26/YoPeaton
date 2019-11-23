using UnityEngine;
using System.Text.RegularExpressions;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private Animator directionalsAnimator;
    private IMovable movableComponent;
    private AITransitionsController transitionsController;
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

        }
    }
    #endregion

    public void SetCurrentAnimation(Vector3 direction)
    {
        //Debug.LogError(direction);
        //Debug.LogError(direction.x);
        //Debug.LogError(direction.y);
        if(movableComponent.GetEntity.GetEntityType == EntityType.Pedestrian)
        {
            if (direction == new Vector3(1.0f, 0f, 0f))
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

            else if (direction == new Vector3(-1.0f, 0f, 0f))
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

            if (direction == new Vector3(0f, 1f, 0f))
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

            else if (direction == new Vector3(0f, -1f, 0f))
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

        if (movableComponent.GetEntity.GetEntityType == EntityType.Car)
        {
            if (direction == new Vector3(1.0f, 0f, 0f))
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

    private void OnDirectionalStop(Vector3 _directional)
    {
        if (directionalsAnimator)
        {
            directionalsAnimator.SetBool("Left", false);
            directionalsAnimator.SetBool("Right", false);
        }
    }
}