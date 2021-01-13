using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaterPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class Beaker
    {
        public int m_MaxVolume;
        [ShowOnly]
        public int m_CurrentVolume;

        public bool m_Actived;
        public List<Sprite> m_Images;

        [ReadOnly(true)]
        public SpriteRenderer m_Renderer;
        [ReadOnly(true)]
        public SpriteRenderer m_OutLine;

        public Beaker()
        {
            m_CurrentVolume = 1;
            m_Actived = false;

            m_Images = new List<Sprite>();
        }

        public void OnBeakerActived()
        {
            m_Actived = true;

            m_OutLine.gameObject.SetActive(true);
        }
        public void OffBeakerActived()
        {
            m_Actived = false;

            m_OutLine.gameObject.SetActive(false);
        }

        public void AddWater()
        {
            m_CurrentVolume = m_MaxVolume;
            UpdateImage();
        }
        public void ClearBeaker()
        {
            m_CurrentVolume = 0;
            UpdateImage();
        }

        public void UpdateImage()
        {
            m_Renderer.sprite = m_Images[m_CurrentVolume];
            OffBeakerActived();
        }
    }

    [ReadOnly(true)]
    public Beaker m_Beaker300;
    [ReadOnly(true)]
    public Beaker m_Beaker500;

    public UnityEvent m_Correct;

    public void OnClick300Beaker()
    {
        if (!m_Beaker300.m_Actived && !m_Beaker500.m_Actived)
        {
            m_Beaker300.OnBeakerActived();
            m_Beaker500.OffBeakerActived();
        }
        else if (m_Beaker300.m_Actived)
        {
            m_Beaker300.OffBeakerActived();
        }
        else if (m_Beaker500.m_Actived)
        {
            while (m_Beaker300.m_CurrentVolume < m_Beaker300.m_MaxVolume &&
                   m_Beaker500.m_CurrentVolume > 0)
            {
                ++m_Beaker300.m_CurrentVolume;
                --m_Beaker500.m_CurrentVolume;
            }

            m_Beaker300.UpdateImage();
            m_Beaker500.UpdateImage();
        }

        if (m_Beaker500.m_CurrentVolume == 4)
        {
            m_Correct?.Invoke();
        }
    }
    public void OnClick500Beaker()
    {
        if (!m_Beaker300.m_Actived && !m_Beaker500.m_Actived)
        {
            m_Beaker300.OffBeakerActived();
            m_Beaker500.OnBeakerActived();
        }
        else if (m_Beaker300.m_Actived)
        {
            while (m_Beaker500.m_CurrentVolume < m_Beaker500.m_MaxVolume &&
                   m_Beaker300.m_CurrentVolume > 0)
            {
                --m_Beaker300.m_CurrentVolume;
                ++m_Beaker500.m_CurrentVolume;
            }

            m_Beaker300.UpdateImage();
            m_Beaker500.UpdateImage();
        }
        else if (m_Beaker500.m_Actived)
        {
            m_Beaker500.OffBeakerActived();
        }

        if (m_Beaker500.m_CurrentVolume == 4)
        {
            m_Correct?.Invoke();
        }
    }

    public void OnClickWaterTap()
    {
        if (m_Beaker300.m_Actived)
        {
            m_Beaker300.AddWater();
        }
        else if (m_Beaker500.m_Actived)
        {
            m_Beaker500.AddWater();
        }
    }
    public void OnClickWaterSpout()
    {
        if (m_Beaker300.m_Actived)
        {
            m_Beaker300.ClearBeaker();
        }
        else if (m_Beaker500.m_Actived)
        {
            m_Beaker500.ClearBeaker();
        }
    }
}
