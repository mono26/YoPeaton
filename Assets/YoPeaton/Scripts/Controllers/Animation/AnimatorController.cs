using UnityEngine;
using System.Text.RegularExpressions;

public class AnimatorController : MonoBehaviour
{
    protected Animator animator;
    private IMovable movableComponent;
    protected AITransitionsController transitionsController;
    [SerializeField]
    protected EntityController entity;

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

    protected virtual void OnEnable()
    {
        if (movableComponent != null)
        {
            movableComponent.AddOnMovement(OnMovement);
        }
    }

    protected virtual void OnDisable()
    {
        if (movableComponent != null)
        {
            movableComponent.RemoveOnMovement(OnMovement);
        }
    }
    #endregion

    public virtual void OnMovement(Vector3 _direction)
    {
        
    }
}