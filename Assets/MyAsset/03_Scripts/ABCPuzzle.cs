using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ABCPuzzle : MonoBehaviour
{

    string[] ABCD = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    string[] abcd = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    string[] Pulz_2 = {"A","C","D","E","I","L","N","R","S"};

    public TextMeshProUGUI m_Text1;
    public TextMeshProUGUI m_Text2;

    public List<TextMeshProUGUI> m_Text3=new List<TextMeshProUGUI>();


    // 입력한 문자
    public string m_InputString1;
    public string m_InputString2;
    public string m_InputString3;

    
    int[] index3 = { 0, 0, 0, 0, 0, 0, 0 };
    // 정답 숫자
    [ReadOnly(true)]
    public string m_AnswerString1;
    [ReadOnly(true)]
    public string m_AnswerString2;
    [ReadOnly(true)]
    public string m_AnswerString3;

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

    public void Pulz_science(int num)
    {
        if(num>0)
        {
            index3[num - 1] = index3[num - 1] + 1;
            if (index3[num - 1] > Pulz_2.Length - 1)
                index3[num - 1] = 0;
            else if (index3[num - 1] < 0)
                index3[num - 1] = Pulz_2.Length - 1;

            m_Text3[num - 1].text = Pulz_2[index3[num - 1]];
        }
        else
        {
            num = num * -1;
            index3[num - 1] = index3[num - 1] -1;
            if (index3[num - 1] > Pulz_2.Length - 1)
                index3[num - 1] = 0;
            else if (index3[num - 1] < 0)
                index3[num - 1] = Pulz_2.Length - 1;

            m_Text3[num - 1].text = Pulz_2[index3[num - 1]];
        }
        string temp="";
        foreach (var i in m_Text3)
        {
            temp =temp + i.text;
        }
        if(temp == m_AnswerString3)
        {
            m_Correct?.Invoke();
            Debug.Log("정답");
        }
        m_InputString3 = temp;
    }


    void InCorrectProcess()
    {
        //m_InputNum = 0;
        //m_Text.text = "";
        //Debug.Log("오답");
    }


}
