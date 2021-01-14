using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InputPuzzle : MonoBehaviour
{
    [ReadOnly(true)]
    public TextMeshProUGUI m_Text;

    [Header("원본과 같게 하려면 0으로")]
    [Header("글자간 간격")]
    public float m_Distance = 70f;

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

    const int Ascii_Number_Min = 48;
    const int Ascii_Number_Max = 57;
    const int Ascii_Alphabet_Upper_Min = 65;
    const int Ascii_Alphabet_Upper_Max = 90;
    const int Ascii_Alphabet_Lower_Min = 97;
    const int Ascii_Alphabet_Lower_Max = 122;

    private void Awake()
    {
        __Initialize();
    }

    public void __Initialize()
    {
        if (m_Text != null)
        {
            m_InputStr = m_Text.text;
            SetText();
        }

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
                SetText();
            }
        }
        else
        {
            m_InputStr += str;

            if (m_Text != null)
            {
                SetText();
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
            m_InputStr = m_Text.text;
        }

        int digit = m_InputStr.Length;

        string Ascii = "";

        for (int i = 0; i < digit - 1; ++i)
        {
            char temp_Char = m_InputStr[i];
            int temp_Int = System.Convert.ToInt32(temp_Char);
            string temp_Ascii = temp_Int.ToString();

            Ascii += temp_Ascii;
            Ascii += '_';
        }

        Ascii += System.Convert.ToInt32(m_InputStr[digit - 1]).ToString();

        string[][] SplitString = new string[2][];
        SplitString[0] = Ascii.Split('_');
        SplitString[1] = str.Split(' ');

        int[] SplitInt = new int[digit];

        for (int i = 0; i < digit; ++i)
        {
            SplitInt[i] = int.Parse(SplitString[0][i]) + int.Parse(SplitString[1][i]);

            if (SplitInt[i] == Ascii_Number_Min - 1)
                SplitInt[i] = Ascii_Number_Max;
            else if (SplitInt[i] == Ascii_Number_Max + 1)
                SplitInt[i] = Ascii_Number_Min;
            else if (SplitInt[i] == Ascii_Alphabet_Upper_Min - 1)
                SplitInt[i] = Ascii_Alphabet_Upper_Max;
            else if (SplitInt[i] == Ascii_Alphabet_Upper_Max + 1)
                SplitInt[i] = Ascii_Alphabet_Upper_Min;
            else if (SplitInt[i] == Ascii_Alphabet_Lower_Min - 1)
                SplitInt[i] = Ascii_Alphabet_Lower_Max;
            else if (SplitInt[i] == Ascii_Alphabet_Lower_Max + 1)
                SplitInt[i] = Ascii_Alphabet_Lower_Min;
        }

        m_InputStr = "";

        for (int i = 0; i < digit; ++i)
        {
            m_InputStr += System.Convert.ToChar(SplitInt[i]);
        }

        if (m_Text != null)
        {
            SetText();
        }

        if (m_InputStr == m_AnswerStr)
        {
            m_Correct?.Invoke();
        }
    }

    void SetText()
    {
        string space = $"<mspace={m_Distance}>";
        space += m_InputStr;
        space += "</mspace>";
        m_Text.text = space;
    }

    void InCorrectProcess()
    {
        m_Text.text = m_InputStr = "";
        Debug.Log("오답");
    }
}
