using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum E_Direction
{
    North,
    East,
    South,
    West,
    Max
}
  
public class DirectionManager : Singleton<DirectionManager>
{
    public Image m_LeftImage;
    public Image m_RightImage;
    public Image m_UpImage;
    public Image m_DownImage;

    public List<Vector3> m_StandardPos;
    public MyDictionary<E_Direction, Vector3> m_Positions;

    List<Image> m_DirImages;
    Camera m_Camera;
    Vector3 m_StandardVector;
    E_Direction m_CurrentDir;

    public override void __Initialize()
    {
        m_DirImages = new List<Image>();

        m_DirImages.Add(m_LeftImage);
        m_DirImages.Add(m_RightImage);
        m_DirImages.Add(m_UpImage);
        m_DirImages.Add(m_DownImage);

        m_LeftImage.gameObject.SetActive(true);
        m_RightImage.gameObject.SetActive(true);
        m_UpImage.gameObject.SetActive(false);
        m_DownImage.gameObject.SetActive(false);

        // 이동시킬 카메라
        m_Camera = Camera.main;

        // 초기 값
        m_StandardVector = Vector3.zero;
        m_StandardVector.z = m_Camera.transform.position.z;

        // 현재 방향
        m_CurrentDir = E_Direction.North;

        // 카메라 이동
        CameraMoveToDir();
    }

    public void CameraMoveToDir()
    {
        m_Camera.transform.position = m_StandardPos[__GameManager.Instance.m_CurrentStage] + m_Positions[m_CurrentDir] + m_StandardVector;
    }
    public void TurnRight()
    {
        m_CurrentDir = (E_Direction)((int)(m_CurrentDir + 1) % (int)E_Direction.Max);
    }
    public void TurnLeft()
    {
        m_CurrentDir = (E_Direction)((int)(m_CurrentDir + (int)E_Direction.Max - 1) % (int)E_Direction.Max);
    }

    private void Update()
    {
        Vector2 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);

        foreach (var item in m_DirImages)
        {
            Vector2 itemPos = item.transform.position;

            Color temp = item.color;
            temp.a = 1 - Mathf.Clamp01(Vector2.Distance(itemPos, mousePos)) * 0.5f;
            item.color = temp;
        }
    }
}
