using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class BunchBase : MonoBehaviour
{
    public List<Key> keys = new List<Key>();
    //only user by player bunch but easier to but it there
    protected int selectedKey = 0;
    public const int MAX_KEYS = 3;
    public UnityEvent onBunchUpdated = new UnityEvent();

    public Key GetKeyAtPosition(int keyPosition)
    {
        if (keys.Count <= 0)
            return null;

        int id = selectedKey + keyPosition;
        id %= keys.Count;

        return keys[id];
    }

    public bool HasKeyOfLength(int length)
    {
        return keys.Any(k => k.length == length);
    }

    public void AddKeys(List<Key> keysToAdd)
    {
        keys.AddRange(keysToAdd);
        onBunchUpdated.Invoke();
    }

    public void AddKey(Key keyToAdd)
    {
        keys.Add(keyToAdd);
        onBunchUpdated.Invoke();
    }
}
