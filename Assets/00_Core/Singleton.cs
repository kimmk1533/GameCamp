using UnityEngine;

// https://202psj.tistory.com/1247 [알레폰드의 IT 이모저모]
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                if (obj == null)
                {
                    obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                else
                {
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    public abstract void __Initialize();

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}