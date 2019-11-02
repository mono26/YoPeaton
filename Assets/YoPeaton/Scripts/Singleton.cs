using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance {
        get {
            if (!instance) {
                instance = new GameObject().AddComponent<T>();
            }
            return instance;
        }
    }

    protected virtual void Awake() {
        if (!instance) {
            instance = this as T;
        }
        else if (!instance.Equals(this as T)) {
            Destroy(this);
        }
    }
}
