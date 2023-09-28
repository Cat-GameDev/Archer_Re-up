using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UICanvas
{
    public void PlayButton()
    {
        LevelManager.Instance.OnStartGame();

        UIManager.Ins.OpenUI<Gameplay>();
        Close(0f);
    }

    public void ShopButton()
    {

    }

    public void WeaponButton()
    {

    }

    public void SettingButton()
    {

    }
}
