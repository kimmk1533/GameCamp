using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightManager : Singleton<LightManager>
{
    public List<Light2D> light_lst = new List<Light2D>();
    public List<Color> lightcolor_lst = new List<Color>();

    [ShowOnly, SerializeField]
    Light2D tmpLight;    //selected obj스크립트의 Start Event에서 인자값을 하나밖에 못받기 때문에 temp로 받아서 저장해 사용.

    //묶음변수.
    List<Light2D> update_light_lst = new List<Light2D>();
    List<Color> update_lightcolor_lst = new List<Color>();
    List<float> update_inensity_lst = new List<float>();

    public override void __Initialize()
    {
        
    }

    //사용할 라이트 저장.
    public void SetTmpLight(int _light)
    {
        tmpLight = light_lst[_light];
    }
    public void SetTmpLight_Light(Light2D _light)
    {
        tmpLight = _light;
    }
    void UpdateValue(Light2D _light, Color _color, float _intensity)    //변경할 사항 리스트에 add.
    {
        update_light_lst.Add(_light);
        update_lightcolor_lst.Add(_color);
        update_inensity_lst.Add(_intensity);
    }
    //빛의 색 변경.
    void ChangeLightColor(Light2D _light, Color _color)
    {
        //_light.color = _color;
        UpdateValue(_light, _color, _light.intensity);
    }
    public void ChangeLightColor(Color _color)
    {
        ChangeLightColor(tmpLight, _color);
    }
    public void ChangeLightColor(int _color)
    {
        ChangeLightColor(tmpLight, lightcolor_lst[_color]);
    }
    public void SetIntensity(float _intensity) //밝기의 정도.
    {
        //tmpLight.intensity = _size;
        UpdateValue(tmpLight, tmpLight.color, _intensity);
    }
    
    public void StartUpdateSetting(float _speed)
    {
        StopCoroutine(StartUpdateSetting_Coroutine(_speed));
        StartCoroutine(StartUpdateSetting_Coroutine(_speed));
    }
    IEnumerator StartUpdateSetting_Coroutine(float _speed)
    {
        bool whilebreak = false;
        bool[] color_change = new bool[update_light_lst.Count];
        bool[] intensity_change = new bool[update_light_lst.Count];
        if (update_light_lst.Count > 0)
        {
            //초기값 세팅.
            for (int i = 0; i < update_light_lst.Count; i++)
            {
                color_change[i] = false;
                intensity_change[i] = false;
            }

            while (!whilebreak)
            {
                for (int i = 0; i < update_light_lst.Count; i++)
                {
                    //색 Lerp.
                    if (!color_change[i])
                    {
                        update_light_lst[i].color = Color.Lerp(update_light_lst[i].color, update_lightcolor_lst[i], Time.deltaTime * _speed );
                        if (update_light_lst[i].color.r < update_lightcolor_lst[i].r + Time.deltaTime * _speed && update_light_lst[i].color.r > update_lightcolor_lst[i].r - Time.deltaTime * _speed)
                        {
                            if (update_light_lst[i].color.g < update_lightcolor_lst[i].g + Time.deltaTime * _speed && update_light_lst[i].color.g > update_lightcolor_lst[i].g - Time.deltaTime * _speed)
                            {
                                if (update_light_lst[i].color.b < update_lightcolor_lst[i].b + Time.deltaTime * _speed && update_light_lst[i].color.b > update_lightcolor_lst[i].b - Time.deltaTime * _speed)
                                {
                                    update_light_lst[i].color = update_lightcolor_lst[i];
                                    color_change[i] = true;
                                }
                            }
                        }
                    }

                    //밝기 값 Lerp.
                    if (!intensity_change[i])
                    {
                        update_light_lst[i].intensity = Mathf.Lerp(update_light_lst[i].intensity, update_inensity_lst[i], Time.deltaTime * _speed);
                        if (update_light_lst[i].intensity < update_inensity_lst[i] + Time.deltaTime * _speed && update_light_lst[i].intensity > update_inensity_lst[i] - Time.deltaTime * _speed)
                        {
                            update_light_lst[i].intensity = update_inensity_lst[i];
                            intensity_change[i] = true;
                        }
                    }
                }

                //while문 탈출 체크.
                for (int i = 0; i < update_light_lst.Count; i++)
                {
                    if (!color_change[i])
                    {
                        break;
                    }
                    else if (!intensity_change[i])
                    {
                        break;
                    }
                    else if (i + 1 == update_light_lst.Count)
                    {
                        whilebreak = true;
                    }
                }

                yield return null;
            }
        }

        update_light_lst.Clear();
        update_lightcolor_lst.Clear();
        update_inensity_lst.Clear();
        Debug.Log("빛 변경 코루틴 종료.");
    }
}
