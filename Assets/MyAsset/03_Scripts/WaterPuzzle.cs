using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class WaterPuzzle : MonoBehaviour
{
    // Start is called before the first frame update

    public UnityEvent m_StartEvent;
    // 액션 종료 시 호출할 이벤트
    public UnityEvent m_EndEvent;

    public List<Sprite> bk500 = null;
    public List<Sprite> bk300 = null;
    public List<GameObject> Effect = null;


    public GameObject beaker300=null;
    public GameObject beaker500=null;
    public GameObject water_tap = null;

    public int addwater = 5;
    public int bk5 = 0;
    public int bk3 = 0;

    public UnityEvent m_Correct;
    public UnityEvent m_InCorrect;

    public GameObject Selectobj;

    [ShowOnly]
    public bool Active500 = false;
    [ShowOnly]
    public bool Active300 = false;
    bool swap = false;


    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        m_InCorrect.AddListener(new UnityAction(InCorrectProcess));

    }

    // Update is called once per frame
    void Update()
    {
        img_renewal();
    }

    public void InPutM()
    {
        Selectobj = GetObject(0);
        if (Selectobj != null)
        {
            GameObject tempobj = Selectobj;
            swap = false;
            if (tempobj.name== "water tap")
            {
                if (Active500)
                {
                    bk5 = 5;
                    Active500 = false;
                }
                else if (Active300)
                {
                    bk3 = 3;
                    Active300 = false;
                }
            }
            else if(tempobj.name == "waterspout")
            {
                if (Active500)
                {
                    bk5 = 0;
                    Active500 = false;
                }
                else if (Active300)
                {
                    bk3 = 0;
                    Active300 = false;
                }
            }

            
            if (Active500)
            {
                if (tempobj.name== "300B")
                {

                    while(bk5 > 0)
                    {
                        if (bk3 >= 3)
                        {
                            bk3 = 3;
                            break;
                        }
                        bk5--;
                        bk3++;
                        
                    }

                    swap = true;
                    Active500 = false;
                    Active300 = false;
                    effect_renewal();
                }
            }

            else if (Active300)
            {
                if (tempobj.name == "500B")
                {
                    while (bk3 > 0)
                    {
                        if (bk5 >= 5)
                        {
                            bk5 = 5;
                            break;
                        }
                        bk3--;
                        bk5++;
                        
                    }
                    swap = true;
                    Active500 = false;
                    Active300 = false;
                    effect_renewal();
                }
            }

            if (!swap)
            {
                if (tempobj.name == "500B")
                {
                    Active500 = true;
                }
                else
                {
                    Active500 = false;
                }
                if (tempobj.name == "300B")
                {
                    Active300 = true;
                }
                else
                {
                    Active300 = false;
                }
            }

            if (bk5 == 4)
            {
                m_Correct?.Invoke();
            }

        }

    }

    void img_renewal()
    {
        if (Active500)
            Effect[0].SetActive(true);
        else if(Active300)
            Effect[1].SetActive(true);
        beaker500.GetComponent<SpriteRenderer>().sprite = bk500[bk5];
        beaker300.GetComponent<SpriteRenderer>().sprite = bk300[bk3];
    }

    void effect_renewal()
    {
        foreach (var item in Effect)
        {
            item.SetActive(false);
        }
    }


    public GameObject GetObject(int layer = -1)
    {
        GameObject target = null;

        int mask = 1 << layer;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit;
        hit = layer == -1 ? Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity) : Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, mask);

        if(hit)
        {
            target = hit.collider.gameObject;
        }
        return target;
    }

    void InCorrectProcess()
    {

    }

}
