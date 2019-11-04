using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionMethods
{
    /// <summary>
    /// Gets a component recursively through the parents.
    /// </summary>
    /// <param name="target">Transform to get parents from.</param>
    /// <typeparam name="T">Type of component to look for.</typeparam>
    /// <returns>Returns first component found.</returns>
    public static T GetComponentInParentRecursive<T>(this Transform target) where T : MonoBehaviour {
        T parentComponent = target.parent.GetComponent<T>();
        if (!parentComponent) {
            parentComponent = GetComponentInParentRecursive<T>(target.parent);
        }
        return parentComponent;
    }
}
