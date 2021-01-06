using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인스펙터에 보이기 위한 유사 딕셔너리 (무거움)
/// </summary>
/// <typeparam name="TKey">키</typeparam>
/// <typeparam name="TValue">값</typeparam>
[System.Serializable]
public class MyDictionary<TKey, TValue>
{
    [System.Serializable]
    public class MyData
    {
        [ReadOnly(true)]
        public TKey key;
        public TValue value;
    }

    [SerializeField]
    List<MyData> m_Dictionary;

    public MyDictionary()
    {
        m_Dictionary = new List<MyData>();
    }

    public TValue this[TKey key]
    {
        get
        {
            foreach (var item in m_Dictionary)
            {
                if (item.key.Equals(key))
                {
                    return item.value;
                }
            }

            return default(TValue);
        }
        set
        {
            foreach (var item in m_Dictionary)
            {
                if (item.key.Equals(key))
                {
                    item.value = value;
                    return;
                }
            }
        }
    }
    public int Count
    {
        get
        {
            return m_Dictionary.Count;
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (CheckKey(key))
        {
            Debug.LogError($"Add({key}, {value}) : 딕셔너리에 동일한 키 값이 존재합니다.");
            return;
        }

        MyData temp = new MyData();
        temp.key = key;
        temp.value = value;

        m_Dictionary.Add(temp);
    }
    public void Clear()
    {
        m_Dictionary.Clear();
    }
    public bool ContainsKey(TKey key)
    {
        return CheckKey(key);
    }
    public bool ContainsValue(TValue value)
    {
        return CheckValue(value);
    }
    public bool Remove(TKey key)
    {
        for (int i = m_Dictionary.Count - 1; i >= 0; --i)
        {
            if (m_Dictionary[i].key.Equals(key))
            {
                m_Dictionary.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    // try, catch, throw 추후 공부해서 수정
    private bool CheckKey(TKey key)
    {
        foreach (var item in m_Dictionary)
        {
            if (item.key.Equals(key))
            {
                return true;
            }
        }

        return false;
    }
    private bool CheckValue(TValue value)
    {
        foreach (var item in m_Dictionary)
        {
            if (item.value.Equals(value))
            {
                return true;
            }
        }

        return false;
    }
}
