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

    public GameObject beaker300=null;
    public GameObject beaker500=null;
    public GameObject water_tap = null;

    public int addwater = 5;
    public int bk5 = 0;
    public int bk3 = 0;

    GameObject Selectobj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //InPutM();
    }

    void InPutM()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Selectobj = GetObject(0);
            if (Selectobj != null)
            {
                GameObject tempobj = Selectobj.transform.FindChildren("select").gameObject;
                Debug.Log(tempobj.name);
            }
            else
                return;
              
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
            Debug.Log(target.name);
        }
        return target;
    }
  
}
