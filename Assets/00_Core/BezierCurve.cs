using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BezierCurve
{
    public List<Transform> m_Targets;
    public List<Vector3> m_Points;
    public List<float> m_Time;

    public void __Initialize()
    {
        m_Targets = new List<Transform>();
        m_Points = new List<Vector3>();
        m_Time = new List<float>();
    }

    #region 디버깅 용도
    [Min(1)]
    public int m_PathDensity = 10;

    // 유니티 내부 OnDrawGizmos() 함수에 사용하면 됨
    public void OnDrawGizmos()
    {
        Color tempColor = Gizmos.color;
        Gizmos.color = Color.red;

        foreach (var item in m_Points)
        {
            Gizmos.DrawSphere(item, 0.1f);
        }
        for (float i = 0f; i <= 1f; i += 1f / m_PathDensity)
        {
            Gizmos.DrawWireSphere(GetBezier(i), 0.1f);
        }

        Gizmos.color = tempColor;
    }
    #endregion

    public Vector3 GetBezier(float t)
    {
        Vector3 result = new Vector3();

        int N = m_Points.Count - 1;

        for (int i = 0; i <= N; ++i)
        {
            float a = Mathf.Pow(t, i);
            float b = Mathf.Pow(1 - t, N - i);
            float c = a * b;

            Vector3 temp = m_Points[i] * MyMathf.Combination(N, i) * c;

            result += temp;
        }

        return result;
    }
}