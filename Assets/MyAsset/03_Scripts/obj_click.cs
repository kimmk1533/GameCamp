using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class obj_click : MonoBehaviour
{

    GameObject target;
    [SerializeField]
    Canvas UIcanvas=null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
    }
     
    void CastRay()
    {
        target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (hit.collider != null && hit.collider.transform == this.transform)
        {
            target = hit.collider.gameObject;
            Debug.Log(this.gameObject.name);
            UIcanvas.gameObject.SetActive(true);
        }
        else
        {
            UIcanvas.gameObject.SetActive(false);
        }
    }


}
