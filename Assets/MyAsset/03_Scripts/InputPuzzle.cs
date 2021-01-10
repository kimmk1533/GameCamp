using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InputPuzzle : MonoBehaviour
{
    public TextMeshProUGUI m_Text;

    // 입력한 숫자
    [ShowOnly]
    public int m_InputNum;
    // 정답 숫자
    [ReadOnly(true)]
    public int m_AnswerNum;

    public UnityEvent m_Correct;
    public UnityEvent m_InCorrect;

    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        m_Text.text = "";

        m_InCorrect.AddListener(new UnityAction(InCorrectProcess));
    }

    public void AddNum(int n)
    {
        if (m_InputNum == m_AnswerNum)
            return;

        // 첫 입력
        if (m_InputNum == 0)
        {
            m_InputNum = n;
            m_Text.text = m_InputNum.ToString();
        }
        else
        {
            string InputString = m_InputNum.ToString();
            string nString = n.ToString();

            m_InputNum = int.Parse(InputString + nString);
            m_Text.text = m_InputNum.ToString();

            // 정답 확인
            if (m_InputNum.ToString().Length == m_AnswerNum.ToString().Length)
            {
                if (m_InputNum == m_AnswerNum)
                {
                    m_Correct?.Invoke();
                    Debug.Log("정답");
                }
                else
                {
                    m_InCorrect?.Invoke();
                }
            }
        }
    }

    void InCorrectProcess()
    {
        m_InputNum = 0;
        m_Text.text = "";
        Debug.Log("오답");
    }
}
