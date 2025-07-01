using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPrefab : MonoBehaviour
{
    private int killCount = 0;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI killCountText;

    public void SetInfoPrefab(string getName, int getKillCount)
    {
        killCount = getKillCount;
        nameText.text = getName;
        killCountText.text = killCount.ToString();
    }

    public void UpdateKillCount()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }

    public void UpdateDead()
    {
        this.GetComponent<Image>().color = new Color(0, 0, 0);
    }

    public int GetKillCount()
    {
        return killCount; 
    }
}
