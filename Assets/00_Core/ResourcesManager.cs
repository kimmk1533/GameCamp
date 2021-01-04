using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourcesPath
{
    public string ResourceName;
    public string ResourcePath;
}

public class ResourcesManager : Singleton<ResourcesManager>
{
    public List<ResourcesPath> m_Paths;

    Dictionary<string, Dictionary<string, GameObject[]>> m_Prefabs = null;
    Dictionary<string, Dictionary<string, Sprite[]>> m_Sprites = null;

    public override void __Initialize()
    {
        m_Prefabs = new Dictionary<string, Dictionary<string, GameObject[]>>();
        m_Sprites = new Dictionary<string, Dictionary<string, Sprite[]>>();

        m_Prefabs = LoadAll<GameObject>();
        m_Sprites = LoadAll<Sprite>();
    }

    Dictionary<string, Dictionary<string, T[]>> LoadAll<T>() where T : Object
    {
        Dictionary<string, Dictionary<string, T[]>> result = new Dictionary<string, Dictionary<string, T[]>>();
        string key;
        T[] tempArray;
        List<T> tempList = new List<T>();

        for (int i = 0; i < m_Paths.Count; ++i)
        {
            result[m_Paths[i].ResourceName] = new Dictionary<string, T[]>();

            #region 프리팹 로드
            tempList.Clear();
            tempArray = Resources.LoadAll<T>("Prefabs/" + m_Paths[i].ResourcePath);

            if (tempArray.Length > 0)
            {
                key = tempArray[0].name;

                for (int j = 0; j < tempArray.Length; ++j)
                {
                    if (tempArray[j].name != key)
                    {
                        result[m_Paths[i].ResourceName].Add(key, tempList.ToArray());
                        key = tempArray[j].name;
                        tempList.Clear();
                    }

                    tempList.Add(tempArray[j]);
                }

                result[m_Paths[i].ResourceName].Add(key, tempList.ToArray());
            }
            #endregion

            #region 스프라이트 로드
            tempList.Clear();
            tempArray = Resources.LoadAll<T>("Sprites/" + m_Paths[i].ResourcePath);

            if (tempArray.Length > 0)
            {
                key = tempArray[0].name.Split('_')[0];

                for (int j = 0; j < tempArray.Length; ++j)
                {
                    if (tempArray[j].name.Split('_')[0] != key)
                    {
                        result[m_Paths[i].ResourceName].Add(key, tempList.ToArray());
                        key = tempArray[j].name.Split('_')[0];
                        tempList.Clear();
                    }

                    tempList.Add(tempArray[j]);
                }

                result[m_Paths[i].ResourceName].Add(key, tempList.ToArray());
            }
            #endregion
        }

        return result;
    }

    public int GetPrefabCount(string name)
    {
        int count = 0;

        List<string> keys = new List<string>(m_Prefabs.Keys);

        foreach (var item in keys)
        {
            if (item.Contains(name))
            {
                ++count;
            }
        }

        return count;
    }
    public int GetSpriteCount(string name)
    {
        int count = 0;

        List<string> keys = new List<string>(m_Sprites.Keys);

        foreach (var item in keys)
        {
            if (item.Contains(name))
            {
                ++count;
            }
        }

        return count;
    }

    public GameObject[] GetPrefabs(string path)
    {
        GameObject[] prefabs = new GameObject[m_Prefabs[path].Count];

        for (int i = 0; i < prefabs.Length; ++i)
        {
            for (int j = 0; j < m_Prefabs[name].Count; ++j)
            {
                prefabs[i] = m_Prefabs[name][m_Prefabs[name].Keys.ToString()][j];
            }

        }
        return prefabs;
    }
    public GameObject[] GetPrefabs(string name, string file)
    {
        return m_Prefabs[name][file];
    }
    public Sprite[] GetSprites(string name, string file)
    {
        if (m_Sprites.ContainsKey(name) &&
            m_Sprites[name].ContainsKey(file))
            return m_Sprites[name][file];

        return null;
    }
}
