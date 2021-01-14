using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class imgcheck : MonoBehaviour
{

    public SpriteRenderer img = null;
    public Sprite sprite = null;
    public Sprite nul = null;

    public UnityEvent m_StartEvent;

    float settime = 6f;
    float start_time;
    // Start is called before the first frame update
    void Start()
    {
        start_time = 0;
        m_StartEvent?.Invoke();
    }


    // Update is called once per frame
    void Update()
    {
        



        start_time += Time.deltaTime;

        if (start_time > settime)
        {
            if (img.sprite == sprite)
            {
                img.sprite = nul;
            }
        }
    }
}
