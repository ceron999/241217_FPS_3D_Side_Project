using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : UIBase
{
    public static StatusUI Instance => UIManager.Singleton.GetUI<StatusUI>(UIList.StatusUI);

    public TextMeshProUGUI hpText;

    public void SetHP(float currHp)
    {
        hpText.text = currHp.ToString();
    }
}
