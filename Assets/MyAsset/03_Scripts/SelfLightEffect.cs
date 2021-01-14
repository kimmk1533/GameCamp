using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SelfLightEffect : MonoBehaviour
{
    public int onstagd_id;
    Light2D light;
    float intensity_up, intensity_down;
    public float flash_speed;
    bool light_up = false;
    public bool isFlash = true, isFollowMouse = false;

    private void Awake()
    {
        light = this.GetComponent<Light2D>();
        intensity_up = light.intensity;
        intensity_down = light.intensity / 2;
    }
    void Update()
    {
        if (onstagd_id != __GameManager.Instance.m_CurrentStage)
        {
            this.gameObject.SetActive(false);
        }
        if (isFlash)
        {
            if (!light_up)
            {
                DownFlash();
            }
            else
            {
                UpFlash();
            }
        }
        if (isFollowMouse)
        {
            LightIsFollowMouse();
        }
    }

    public void SetActiveThis()
    {
        gameObject.SetActive(true);
    }

    void UpFlash()
    {
        light.intensity += flash_speed;
        if (light.intensity > intensity_up)
        {
            light.intensity = intensity_up;
            light_up = false;
        }
    }
    void DownFlash()
    {
        light.intensity -= flash_speed;
        if (light.intensity < intensity_down)
        {
            light.intensity = intensity_down;
            light_up = true;
        }
    }

    void LightIsFollowMouse()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 tmp;
        tmp.x = newPosition.x;
        tmp.y = newPosition.y;
        tmp.z = this.transform.position.z;
        this.gameObject.transform.position = tmp;
    }
}
