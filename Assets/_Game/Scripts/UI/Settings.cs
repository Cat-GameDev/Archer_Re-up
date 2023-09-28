using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : UICanvas
{
    public override void Open()
    {
        Time.timeScale = 0;
        base.Open();
    }

    public override void Close(float delayTime)
    {
        Time.timeScale = 1;
        base.Close(0);
    }

    public void ContinueButton()
    {
        Close(0);
    }

    public void HomeButton()
    {
        LevelManager.Instance.Home();
        Close(0);
    }
}
