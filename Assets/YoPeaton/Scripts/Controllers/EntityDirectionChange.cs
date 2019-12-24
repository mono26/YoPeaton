using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDirectionChange : MonoBehaviour
{
    public Action<OnStartDirectionChangeArgs> onStartDirectionChange;
    public Action onStopChangingDirection;

    private EntityController entityReference;

    private bool canTurn = true;

    private void Awake()
    {
        entityReference = GetComponent<EntityController>();
    }

    public void TryChangeDirection(DirectionChange _directionChanger)
    {
        if (canTurn)
        {
            canTurn = false;
            StartCoroutine(ChangeCanTurnValueCR());
            Path nextPath = _directionChanger.GetConnectionFrom(entityReference.GetCurrentPath);
            // nextPathReference = nextPath;
            if (nextPath)
            {
                Vector3 currentDirection = entityReference.GetFollowPathComponent.GetCurrentDirection;
                Vector3 nextDirection = nextPath.GetDirectionAt(nextPath.GetTParameter(transform.position));
                OnStartDirectionChangeArgs directionArgs = new OnStartDirectionChangeArgs
                {
                    CurrentDirection = currentDirection,
                    NextDirection = nextDirection,
                    NextPath = nextPath
                };
                onStartDirectionChange?.Invoke(directionArgs);
            }
        }
    }

    IEnumerator ChangeCanTurnValueCR()
    {
        yield return new WaitForSeconds(2f);
        canTurn = true;
    }
}
