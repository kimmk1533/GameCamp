using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] bool ispause = false;
    public Transform PausePanal;

    public override void __Initialize()
    {

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

    public void PauseButton()
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
}
