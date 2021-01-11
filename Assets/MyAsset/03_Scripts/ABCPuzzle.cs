using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ABCPuzzle : MonoBehaviour
{

    string[] ABCD = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    string[] abcd = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    public TextMeshProUGUI m_Text1;
    public TextMeshProUGUI m_Text2;

    // 입력한 문자
    public string m_InputString1;
    public string m_InputString2;

    // 정답 숫자
    [ReadOnly(true)]
    public string m_AnswerString1;
    [ReadOnly(true)]
    public string m_AnswerString2;

    int index1=0, index2=0;

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

    }

    public void Change1(int num)
    {
        

        index1 = index1 + num;
        if (index1 > ABCD.Length-1)
            index1 = 0;
        else if (index1 < 0)
            index1 = ABCD.Length - 1;

        m_Text1.text = ABCD[index1];
        m_InputString1 = m_Text1.text;
        if (m_InputString1 == m_AnswerString1 && m_InputString2 == m_AnswerString2)
        {
            m_Correct?.Invoke();
            Debug.Log("정답");
        }

    }

    public void Change2(int num)
    {
        
        index2 = index2 + num;
        if (index2 > abcd.Length-1)
            index2 = 0;
        else if (index2 < 0)
            index2 = abcd.Length - 1;

        m_Text2.text = abcd[index2];
        m_InputString2 = m_Text2.text;
        if (m_InputString1 == m_AnswerString1 && m_InputString2 == m_AnswerString2)
        {
            m_Correct?.Invoke();
            Debug.Log("정답");
        }
    }

    void InCorrectProcess()
    {
        //m_InputNum = 0;
        //m_Text.text = "";
        //Debug.Log("오답");
    }

    // 자릿수 변하는 퍼즐에 사용
    //public void AddNum(int num)
    //{
    //    if (m_InputNum == m_AnswerNum)
    //        return;

    //    // 첫 입력
    //    if (m_InputNum == 0)
    //    {
    //        m_InputNum = num;
    //        m_Text.text = m_InputNum.ToString();
    //    }
    //    else
    //    {
    //        string InputString = m_InputNum.ToString();
    //        string nString = num.ToString();

    //        m_InputNum = int.Parse(InputString + nString);
    //        m_Text.text = m_InputNum.ToString();

    //        // 정답 확인
    //        if (m_InputNum.ToString().Length == m_AnswerNum.ToString().Length)
    //        {
    //            if (m_InputNum == m_AnswerNum)
    //            {
    //                m_Correct?.Invoke();
    //                Debug.Log("정답");
    //            }
    //            else
    //            {
    //                m_InCorrect?.Invoke();
    //            }
    //        }
    //    }
    //}

    // 자릿수 고정 퍼즐에 사용

}
