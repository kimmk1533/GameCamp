using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Camera m_Camera;
    public E_Direction m_Dir;

    Dictionary<E_Direction, Vector3> m_Positions;

    public override void __Initialize()
    {
        // 이동시킬 카메라
        if (m_Camera == null)
        {
            m_Camera = Camera.main;
        }

        // 현재 방향
        m_Dir = E_Direction.North;

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
        m_Camera.transform.position = m_Positions[m_Dir];
        Debug.Log(m_Dir);
    }
    public void TurnRight()
    {
        m_Dir = (E_Direction)((int)(m_Dir + 1) % (int)E_Direction.Max);
    }
    public void TurnLeft()
    {
        m_Dir = (E_Direction)((int)(m_Dir + (int)E_Direction.Max - 1) % (int)E_Direction.Max);
    }
}
