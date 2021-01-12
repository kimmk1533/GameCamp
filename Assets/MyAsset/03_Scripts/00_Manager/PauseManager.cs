using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum WindowMode
{
    Window,
    Full
}

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] bool ispause = false;
    public Transform PausePanal;
    Transform UpBar;    //상단 바 버튼들의 부모.
    List<Image> pauseUpButton_img = new List<Image>();  //상단 바 버튼 이미지 컴포넌트 리스트.
    Transform DownPanal;    //하단 패널들의 부모.
    List<GameObject> pauseDownPanal_obj = new List<GameObject>();   //하단 패널 오브젝트 컴포넌트 리스트.
    [ReadOnly(true)]
    public Color m_DefaultButtonColor;  //선택하지 않은 버튼 색.
    [ReadOnly(true)]
    public Color m_SelectButtonColor;  //선택한 버튼 색.
    [SerializeField]
    WindowMode windowMode;  //현재 윈도우 모드.
    public Transform FullScreenButton;
    public Transform WindowScreenButton;
    TextMeshProUGUI ScrollviewButton_txt;

    public override void __Initialize()
    {
        ScrollviewButton_txt = PausePanal.FindChildren("ScrollviewButton").FindChildren("SizeText").GetComponent<TextMeshProUGUI>();
        SetWindowMode(Screen.fullScreen);
        //UpBar 오브젝트 아래 있는 일시정지 패널의 버튼들 리스트에 삽입.
        UpBar = PausePanal.FindChildren("UpBar");
        if (UpBar != null)
        {
            for (int i = 0; i < UpBar.childCount; i++)
            {
                pauseUpButton_img.Add(UpBar.GetChild(i).GetComponent<Image>());
            }
        }
        else
        {
            Debug.LogError("[UpBar] is no [PausePanal]'s child.");
        }
        //DownPanal 오브젝트 아래 있는 패널의 면들 리스트에 삽입.
        DownPanal = PausePanal.FindChildren("DownPanal");
        if (DownPanal != null)
        {
            for (int i = 0; i < DownPanal.childCount; i++)
            {
                pauseDownPanal_obj.Add(DownPanal.GetChild(i).gameObject);
            }
        }
        else
        {
            Debug.LogError("[DownPanal] is no [PausePanal]'s child.");
        }
        DisplayButton(pauseUpButton_img[0].gameObject); //제일 첫번째 버튼(디스플레이 버튼)으로 디폴드 설정.
    }

    //get set
    public bool GetIsPause()
    {
        return ispause;
    }
    public void SetIsPause(bool _pause)
    {
        ispause = _pause;
    }

    public void PauseButton()   //일시정지 버튼 클릭.
    {
        ispause = !ispause;
        if (ispause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        PausePanal.gameObject.SetActive(ispause);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    //다운패널 - 디스플레이 관련 함수.
    public void DisplayButton(GameObject _thisButton)   //상단 바(UpBar)에서 버튼을 입력 시.
    {
        for (int i = 0; i < UpBar.childCount; i++)
        {
            bool downpanal_isactive = false;
            if (pauseUpButton_img[i].gameObject == _thisButton)
            {
                pauseUpButton_img[i].color = m_SelectButtonColor;
                downpanal_isactive = true;
            }
            else
            {
                pauseUpButton_img[i].color = m_DefaultButtonColor;
            }
            if (pauseDownPanal_obj.Count > i)
            {
                if (pauseDownPanal_obj[i] != null)
                {
                    pauseDownPanal_obj[i].SetActive(downpanal_isactive);
                }
            }
        }
    }

    void SetScreenSetting(int width, int height, WindowMode _winMode)    //화면 설정.
    {
        bool winMode = WindowModeToBool(_winMode);
        Screen.SetResolution(width, height, winMode);
        FullScreenButton.FindChildren("FullScreenCol").gameObject.SetActive(WindowModeToBool(windowMode));
        WindowScreenButton.FindChildren("WindowScreenCol").gameObject.SetActive(!WindowModeToBool(windowMode));
        ScrollviewButton_txt.text = width + " x " + height;
        Debug.Log("[해상도] Screen.width: " + Screen.width + ", Screen.height :" + Screen.height + ", Screen.fullScreen: " + Screen.fullScreen);
    }
    public void SetScreenWH(int width, int height)
    {
        SetScreenSetting(width, height, windowMode);
    }
    public void SetWindowMode(bool _winMode)  //윈도우 모드 설정 함수.
    {
        WindowMode winMode = BoolToWindowMode(_winMode);
        windowMode = winMode;
        SetScreenSetting(Screen.width, Screen.height, windowMode);
    }
    public WindowMode BoolToWindowMode(bool _winMode)   //bool 변수를 WindowMode로 변환.
    {
        WindowMode tmp;
        if (_winMode)
            tmp = WindowMode.Full;
        else
            tmp = WindowMode.Window;
        return tmp;
    }
    public bool WindowModeToBool(WindowMode _winMode)   //bool 변수를 WindowMode로 변환.
    {
        bool tmp = false;
        if (_winMode == WindowMode.Full)
            tmp = true;
        else if (_winMode == WindowMode.Window)
            tmp = false;
        else
            Debug.LogError("[_winMode] value is no in [WindowMode]");

        return tmp;
    }

    public void ScrollObjSetActive (GameObject _scrollObj)
    {
        _scrollObj.SetActive(!_scrollObj.activeSelf);
    }
    public void SetScreenWH_Button(string _str) //버튼을 이용한 해상도 값 변경(매개변수가 1개만 받아와져서 string으로 처리, 예: 1920x1080(띄어쓰기 쓰면 안됨)).
    {
        string[] _strsplit = _str.Split('x');
        if (_strsplit.Length != 2)
        {
            Debug.LogError("[_str] 문자열에 문장을 나누는 기준인 x의 개수가 맞지 않음.");
            return;
        }

        SetScreenWH(int.Parse(_strsplit[0]), int.Parse(_strsplit[1]));
    }
}
