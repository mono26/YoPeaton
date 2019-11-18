using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct SerializableDictionary<T1, T2>
{
    [SerializeField]
    private Hashtable dictionary;

    public void AddData(T1 _key, T2 _value)
    {
        dictionary.Add(_key, _value);
    }

    public bool HasKey(T1 _key)
    {
        return dictionary.ContainsKey(_key);
    }

    public T2 GetValue(T1 _key)
    {
        T2 objectToReturn = default(T2);
        if (HasKey(_key))
        {
            objectToReturn = (T2)dictionary[_key];
        }
        else
        {
            DebugController.LogErrorMessage(string.Format("There is no value for that key {0}", _key.ToString()));
        }
        return objectToReturn;
    }

    public void RemoveKey(T1 _key)
    {
        if (HasKey(_key))
        {
            dictionary.Remove(_key);
        }
    }
}
