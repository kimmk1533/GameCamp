using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class Lockjoystick : MonoBehaviour
{
    public List<Sprite> imglst = null;
    public List<Image> images = null;
    public TextMeshProUGUI m_Text;



    int index=0;
    int Maxindex=3;


    public bool m_LeftActive;
    public bool m_RightActive;
    public bool m_UpActive;
    public bool m_DownActive;

    public GameObject stick=null;

    float settingx;
    float settingy;
    bool moving;
    bool turn = false;
    public Transform standard;
    public float movespeed=0.1f;
    

    void Start()
    {
        moving = true;
    }


    void Update()
    {
        move();
    }




    void move()
    {
        float movex = settingx * movespeed * Time.deltaTime;
        float movey = settingy * movespeed * Time.deltaTime;


        if (movex != 0 || movey != 0)
        {
            moving = false;
            stick.transform.position += new Vector3(movex, movey, 0);

            float distance = Vector3.Distance(stick.transform.position, standard.position);
            Debug.Log(distance);
            Debug.Log(standard.position);
            if (distance > 1f)
            {
                turn = true;
                settingx = settingx * -1;
                settingy = settingy * -1;
            }

            if (turn && distance < 0.3f)
            {
                settingx = 0;
                settingy = 0;
                turn = false;
                stick.transform.position = standard.position;
                moving = true;
                m_LeftActive = false;
                m_RightActive = false;
                m_DownActive = false;
                m_UpActive = false;
            }
        }

    }


    public void movig_trans(string set)
    {
        
        switch (set)
        {
            case "U":
                m_UpActive = true;
                ChangeImage(1);
                break;
            case "D":
                m_DownActive = true;
                ChangeImage(2);
                break;
            case "L":
                m_LeftActive = true;
                ChangeImage(3);
                break;
            case "R":
                m_RightActive = true;
                ChangeImage(4);
                break;
        }


        index++;
        index = index_check(index);

        if (moving)
        {
            if (m_LeftActive)
            {
                settingx = -1f;
            }

            else if (m_RightActive)
            {
                settingx = 1f;
            }
            else
                settingx = 0f;

            if (m_UpActive)
            {
                settingy = 1f;
            }
            else if (m_DownActive)
            {
                settingy = -1f;
            }
            else
                settingy = 0f;

        }
    }

    void ChangeImage(int imgindex)
    {
        images[index].sprite = imglst[imgindex];
        images[index].color= new Color(images[index].color.r, 200f, images[index].color.b, 1.0f);
    }

    int index_check(int index)
    {
        int temp = index;
        if (temp > Maxindex)
        {
            temp = 0;
            foreach (var item in images)
            {
                item.sprite = imglst[0];
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0f);
                m_Text.text = "";
            }
        }
        index = temp;
        return index;
    }


}
