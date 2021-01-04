using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 최종 수정 09-26
public class MemoryPool : System.IDisposable
{
    private Queue<GameObject> queue = new Queue<GameObject>();      // 생성한 오브젝트들을 담을 실제 풀
    private GameObject original;        // 풀에 담을 원본
    private int poolSize;               // 초기 풀 사이즈
    private Transform parent = null;    // 생성시 하이어아키 창에서 관리하기 쉽도록 parent 지정

    public int preLoadedPoolSize = 1000;

    // 부모 지정을 하지 않고 생성하는 경우
    public MemoryPool(GameObject _original, int _poolSize)
    {
        original = _original;
        poolSize = _poolSize;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newItem = GameObject.Instantiate(original);  // new            
            newItem.SetActive(false);
            queue.Enqueue(newItem);
        }
    }

    // 부모 지정하여 생성하는 경우
    public MemoryPool(GameObject _original, int _poolSize, Transform parent)
    {
        original = _original;
        poolSize = _poolSize;
        this.parent = parent;

        for (int i = 0; i < (poolSize > preLoadedPoolSize ? preLoadedPoolSize : poolSize); i++)
        {
            GameObject newItem = GameObject.Instantiate(original);  //new
            string[] strs = newItem.name.Split('(');
            newItem.name = strs[0];
            newItem.SetActive(false);
            newItem.transform.SetParent(parent);
            queue.Enqueue(newItem);
        }


        if (poolSize > preLoadedPoolSize)
        {
            //StartCoroutine(MakePool());
        }
    }

    IEnumerator MakePool()
    {
        for (int i = preLoadedPoolSize; i < poolSize; ++i)
        {
            GameObject newItem = GameObject.Instantiate(original);  //new
            string[] strs = newItem.name.Split('(');
            newItem.name = strs[0];
            newItem.SetActive(false);
            newItem.transform.SetParent(parent);
            queue.Enqueue(newItem);
        }

        yield return new WaitForSeconds(0.1f);
    }

    // foreach 문을 위한 반복자
    public IEnumerator GetEnumerator()
    {
        foreach (GameObject item in queue)
            yield return item;
    }

    // 오브젝트 풀이 빌 경우 선택적으로 call
    // 절반만큼 증가
    void ExpandPoolSize()
    {
        int newSize = poolSize + poolSize / 2;
        for (int i = poolSize; i < newSize; i++)
        {
            GameObject newItem = GameObject.Instantiate(original);
            newItem.SetActive(false);
            if (parent != null)
                newItem.transform.SetParent(parent);
            queue.Enqueue(newItem);
        }
        poolSize = newSize;
    }

    // 모든 오브젝트 사용시 추가로 생성할 경우 
    // expand 를 true 로 설정
    public GameObject Spawn(bool expand = true)
    {
        if (queue.Count > 0)
        {
            GameObject item = queue.Dequeue();
            return item.gameObject;
        }
        if (expand)
        {
            ExpandPoolSize();
            GameObject item = queue.Dequeue();
            return item.gameObject;
        }
        else
        {
            Debug.LogWarning("pool size over");
            return null;
        }
    }

    // 회수 작업
    public void DeSpawn(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        gameObject.SetActive(false);
        gameObject.transform.SetParent(parent);
        queue.Enqueue(gameObject);
    }

    // 메모리 해제
    public void Dispose()
    {
        foreach (GameObject item in queue)
            GameObject.Destroy(item);
        queue.Clear();
        queue = null;
    }
}