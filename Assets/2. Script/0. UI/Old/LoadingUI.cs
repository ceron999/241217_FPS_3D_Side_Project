using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : UIBase
{
    public static LoadingUI Instance => UIManager.Singleton.GetUI<LoadingUI>(UIList.LoadingUI);
    public Image LoadingBar;

    public void SetProgress(float getProgress)
    {
        LoadingBar.fillAmount = getProgress / 0.9f;
    }
}
