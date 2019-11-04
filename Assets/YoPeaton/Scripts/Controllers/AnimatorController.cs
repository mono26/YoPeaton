using UnityEngine;
using System.Text.RegularExpressions;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private IMovable movableComponent;

    #region Unity calls
    private void Awake()
    {
        animator = GetComponent<Animator>();
        movableComponent = GetComponent<IMovable>();
    }

    void Start()
    {
        string keyName = movableComponent.GetEntity.GetEntityType.ToString();
        animator.runtimeAnimatorController = AnimatorControllerDispatcher.GetInstance.Request(keyName);
    }

    void OnEnable()
    {
        movableComponent.AddOnMovement(SetCurrentAnimation);
    }

    void OnDisable()
    {
        movableComponent.RemoveOnMovement(SetCurrentAnimation);
    }
    #endregion

    public void SetCurrentAnimation(Vector3 direction)
    {
        //Debug.LogError(direction);
        //Debug.LogError(direction.x);
        //Debug.LogError(direction.y);
        if (direction.x >= 0f && direction.x <= 1f)
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
