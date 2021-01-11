using System.Collections;
using System.Collections.Generic;
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

    public override void __Initialize()
    {
        SetWindowMode(BoolToWindowMode(Screen.fullScreen));
        //UpBar 오브젝트 아래 있는 일시정지 패널의 버튼들 리스트에 삽입.
        UpBar = PausePanal.FindChildren("UpBar");
        if (UpBar != null)
        {
            for (int i = 0; i < UpBar.childCount; i++)
            {
                pauseUpButton_img.Add(UpBar.GetChild(i).GetComponent<Image>());
            }
            SetButtonColor(pauseUpButton_img[0].gameObject);
        }
        else
        {
            Debug.LogError("[UpBar] is no [PausePanal]'s child.");
        }
        //DownPanal 오브젝트 아래 있는 패널의 면들 리스트에 삽입.
        DownPanal = PausePanal.FindChildren("DownPanal");
        if (DownPanal != null)
        {
            for (int i = 0; i < UpBar.childCount; i++)
            {
                pauseDownPanal_obj.Add(UpBar.GetChild(i).gameObject);
            }
        }
        else
        {
            Debug.LogError("[DownPanal] is no [PausePanal]'s child.");
        }
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

    void SetButtonColor(GameObject _selectButton)   //상단 바(UpBar) 버튼 색상 재설정.
    {
        for (int i = 0; i < UpBar.childCount; i++)
        {
            if (pauseUpButton_img[i].gameObject == _selectButton)
            {
                pauseUpButton_img[i].color = m_SelectButtonColor;
            }
            else
            {
                pauseUpButton_img[i].color = m_DefaultButtonColor;
            }
        }
    }
    public void DisplayButton(GameObject _thisButton)   //상단 바(UpBar)에서 버튼을 입력 시.
    {
        SetButtonColor(_thisButton);
    }

    public void SetScreenSetting(int width, int height, WindowMode _winMode)    //화면 설정.
    {
        bool winMode = WindowModeToBool(_winMode);
        Screen.SetResolution(width, height, winMode);
        FullScreenButton.FindChildren("FullScreenCol").gameObject.SetActive(WindowModeToBool(windowMode));
        WindowScreenButton.FindChildren("WindowScreenCol").gameObject.SetActive(!WindowModeToBool(windowMode));
    }
    public void SetWindowMode(WindowMode _winMode)  //윈도우 모드 설정 함수.
    {
        windowMode = _winMode;
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
}
