using System;
using System.Collections;
using System.Collections.Generic;

public class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>

{
    private MyList<KeyValuePair<TKey, TValue>> keyValuePairs = new MyList<KeyValuePair<TKey, TValue>>();

    public void Add(TKey key, TValue value)
    {
        if (ContainsKey(key))
        {
            throw new InvalidOperationException("An item with the same key has already been added.");
        }

        keyValuePairs.Add(new KeyValuePair<TKey, TValue>(key, value));
    }

    public bool ContainsKey(TKey key)
    {
        foreach (var pair in keyValuePairs)
        {
            if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return keyValuePairs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public TValue this[TKey key]
    {
        get
        {
            foreach (var pair in keyValuePairs)
            {
                if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                {
                    return pair.Value;
                }
            }
            throw new KeyNotFoundException("The given key was not present in the dictionary.");
        }
        set
        {
            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(keyValuePairs[i].Key, key))
                {
                    keyValuePairs[i] = new KeyValuePair<TKey, TValue>(key, value);
                    return;
                }
            }
            Add(key, value);
        }
    }

    public bool Remove(TKey key)
    {
        for (int i = 0; i < keyValuePairs.Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(keyValuePairs[i].Key, key))
            {
                keyValuePairs.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public IEnumerable<TKey> Keys
    {
        get { return keyValuePairs.Select(pair => pair.Key); }
    }
}