using UnityEngine;
using System.Text.RegularExpressions;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private IMovable movableComponent;
    private AITransitionsController transitionsController;

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
        movableComponent.AddOnMovement(SetCurrentAnimation);
        if(transitionsController != null)
            transitionsController.onStartedAskingForCross += OnStartedToAskForPass;
    }

    void OnDisable()
    {
        movableComponent.RemoveOnMovement(SetCurrentAnimation);
        if (transitionsController != null)
            transitionsController.onStartedAskingForCross -= OnStartedToAskForPass;
    }
    #endregion

    public void SetCurrentAnimation(Vector3 direction)
    {
        //Debug.LogError(direction);
        //Debug.LogError(direction.x);
        //Debug.LogError(direction.y);
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

    public void OnStartedToAskForPass() {

    }
}