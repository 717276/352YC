using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestList<TKey, TValue> : MonoBehaviour
{
    private Dictionary<TKey, TValue> questDictionary;
    public QuestList()
    {
        questDictionary = new Dictionary<TKey, TValue>();
    }
    public void Add(TKey key, TValue value)
    {
        questDictionary.Add(key, value);
    }

    public bool Remove(TKey key)
    {
        return questDictionary.Remove(key);
    }
    public TValue Get(TKey key)
    {
        return questDictionary.TryGetValue(key, out TValue value) ? value : default;
    }
    public bool ContainsKey(TKey key)
    {
        return questDictionary.ContainsKey(key);
    }
    public int Count()
    {
        return questDictionary.Count;
    }
}
