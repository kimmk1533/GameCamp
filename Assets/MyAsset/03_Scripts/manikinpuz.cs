using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class manikinpuz : MonoBehaviour
{
    public List<GameObject> obj = null;
    public List<bool> active = null;
    public UnityEvent m_Correct;
    public UnityEvent m_InCorrect;

    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        m_InCorrect.AddListener(new UnityAction(InCorrectProcess));

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void check_enable()
    {
        int index = 0;
        foreach (var item in obj)
        {
            if (item.activeSelf)
                active[index] = true;
            index++;
        }

        if(active[0]&& active[1]&& active[2])
            m_Correct?.Invoke();
    }

    void InCorrectProcess()
    {

    }
}
