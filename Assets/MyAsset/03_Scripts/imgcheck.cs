using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class imgcheck : MonoBehaviour
{

    public SpriteRenderer img = null;
    public Sprite nul = null;
    public Sprite change = null;


    public float settime ;
    float start_time;
    public float del_time;

    bool flag = true;
    // Start is called before the first frame update
    void Start()
    {
        start_time = 0;
    }


    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            if (img.sprite == nul)
                change_img();
        }

        else
        {
            
            start_time += Time.deltaTime;
            if (start_time > del_time)
            {
                img.sprite = nul;
                img.gameObject.SetActive(false);
            }
        }

        Debug.Log(start_time);


    }

    void change_img()
    {

        start_time += Time.deltaTime;

        if (start_time > settime)
        {
            img.sprite = change;
            flag = false;
            start_time = 0;
        }

    }
}
