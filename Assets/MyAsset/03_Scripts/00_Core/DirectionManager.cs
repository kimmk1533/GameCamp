using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List<Image> m_DirImages;
    public MyDictionary<E_Direction, Vector3> m_Positions;

    Camera m_Camera;
    float m_StartZ;
    E_Direction m_CurrentDir;

    public override void __Initialize()
    {
        // 이동시킬 카메라
        m_Camera = Camera.main;

        // 카메라 초기 z값
        m_StartZ = m_Camera.transform.position.z;

        // 현재 방향
        m_CurrentDir = E_Direction.North;

        // 카메라 이동
        CameraMove();
    }

    public void CameraMove()
    {
        Vector3 temp = m_Positions[m_CurrentDir];
        temp.z += m_StartZ;
        m_Camera.transform.position = temp;
        Debug.Log(m_CurrentDir);
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
