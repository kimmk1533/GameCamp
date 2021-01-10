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

    [ShowOnly, SerializeField]
    int m_AnswerLength;

    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        m_InCorrect.AddListener(new UnityAction(InCorrectProcess));

        m_AnswerLength = m_AnswerNum.ToString().Length;
    }

    // 자릿수 변하는 퍼즐에 사용
    public void AddNum(int num)
    {
        if (m_InputNum == m_AnswerNum)
            return;

        // 첫 입력
        if (m_InputNum == 0)
        {
            m_InputNum = num;
            m_Text.text = m_InputNum.ToString();
        }
        else
        {
            string InputString = m_InputNum.ToString();
            string nString = num.ToString();

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

    // 자릿수 고정 퍼즐에 사용
    public void ChangeNum(int num)
    {
        if (m_InputNum == m_AnswerNum)
            return;

        int digit = num.ToString().Replace("-", "").Length;
        int index = m_AnswerLength - digit;

        // 숫자를 뺌으로써 자릿수가 변하면
        if (num < 0 && m_InputNum.ToString($"D{m_AnswerLength}")[index] == '0')
        {
            // 그 윗자리를 더해줌
            m_InputNum += (int)Mathf.Pow(10, digit);
        }
        // 숫자를 더함으로써 자릿수가 변하면
        else if (num > 0 && m_InputNum.ToString($"D{m_AnswerLength}")[index] == '9')
        {
            // 그 윗자리를 빼줌
            m_InputNum -= (int)Mathf.Pow(10, digit);
        }

        m_InputNum += num;

        m_Text.text = m_InputNum.ToString($"D{m_AnswerLength}");

        if (m_InputNum == m_AnswerNum)
        {
            m_Correct?.Invoke();
        }
    }

    void InCorrectProcess()
    {
        m_InputNum = 0;
        m_Text.text = "";
        Debug.Log("오답");
    }
}
