using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InputPuzzle : MonoBehaviour
{
    [ReadOnly(true)]
    public TextMeshProUGUI m_Text;

    // 입력한 문자
    [ShowOnly]
    public string m_InputStr;
    // 정답 문자
    [ReadOnly(true)]
    public string m_AnswerStr;

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
        m_InputStr = "";

        m_InCorrect.AddListener(new UnityAction(InCorrectProcess));

        m_AnswerLength = m_AnswerStr.Length;
    }

    private void OnEnable()
    {
        if (m_InputStr == m_AnswerStr)
        {
            this.gameObject.SetActive(false);
            //m_Correct?.Invoke();
        }
    }

    // 자릿수 변하는 퍼즐에 사용
    public void AddStr(string str)
    {
        if (m_InputStr == m_AnswerStr)
            return;

        // 첫 입력
        if (m_InputStr == "")
        {
            m_InputStr = str;

            if (m_Text != null)
            {
                m_Text.text = m_InputStr.ToString();
            }
        }
        else
        {
            m_InputStr += str;

            if (m_Text != null)
            {
                m_Text.text = m_InputStr;
            }

            // 정답 확인
            if (m_InputStr.Length == m_AnswerLength)
            {
                if (m_InputStr == m_AnswerStr)
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
    public void ChangeStr(string str)
    {
        if (m_InputStr == m_AnswerStr)
            return;

        if (m_InputStr == "")
        {
            m_InputStr = "0";
        }

        int digit = str.ToString().Replace("-", "").Length;
        int index = m_AnswerLength - digit;

        int InputNum = int.Parse(m_InputStr);
        int num = int.Parse(str);

        // 숫자를 뺌으로써 자릿수가 변하면
        if (num < 0 && InputNum.ToString($"D{m_AnswerLength}")[index] == '0')
        {
            // 그 윗자리를 더해줌
            InputNum += (int)Mathf.Pow(10, digit);
        }
        // 숫자를 더함으로써 자릿수가 변하면
        else if (num > 0 && InputNum.ToString($"D{m_AnswerLength}")[index] == '9')
        {
            // 그 윗자리를 빼줌
            InputNum -= (int)Mathf.Pow(10, digit);
        }

        InputNum += num;

        m_InputStr = InputNum.ToString();

        if (m_Text != null)
        {
            m_Text.text = InputNum.ToString($"D{m_AnswerLength}");
        }

        if (m_InputStr == m_AnswerStr)
        {
            m_Correct?.Invoke();
        }
    }

    void InCorrectProcess()
    {
        m_InputStr = "";
        m_Text.text = m_InputStr;
        Debug.Log("오답");
    }
}
