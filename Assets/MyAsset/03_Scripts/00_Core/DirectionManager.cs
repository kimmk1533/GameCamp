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
    public E_Direction m_CurrentDir;
    public List<Image> m_DirImages;

    Camera m_Camera;
    Dictionary<E_Direction, Vector3> m_Positions;

    public override void __Initialize()
    {
        // 이동시킬 카메라
        m_Camera = Camera.main;

        // 현재 방향
        m_CurrentDir = E_Direction.North;

        // 방향에 따른 위치들
        m_Positions = new Dictionary<E_Direction, Vector3>();

        m_Positions.Add(E_Direction.North, new Vector3(-30f, 0f, -50f));
        m_Positions.Add(E_Direction.East, new Vector3(-10f, 0f, -50f));
        m_Positions.Add(E_Direction.South, new Vector3(10f, 0f, -50f));
        m_Positions.Add(E_Direction.West, new Vector3(30f, 0f, -50f));

        // 카메라 이동
        CameraMove();
    }

    public void CameraMove()
    {
        m_Camera.transform.position = m_Positions[m_CurrentDir];
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
