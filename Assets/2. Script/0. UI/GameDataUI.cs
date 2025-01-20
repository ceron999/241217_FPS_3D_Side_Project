using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameDataUI : UIBase
{
    public static GameDataUI Instance => UIManager.Singleton.GetUI<GameDataUI>(UIList.GameDataUI);

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI aIText;

    public TextMeshProUGUI timerText;

    public bool isGameEnd = false;
    private float getTime;

    private void Update()
    {
        if(!isGameEnd)
            UpdateTime();
    }

    public void UpdateGameData(int playerCount, int AICount)
    {
        if(playerCount < 0)
            playerCount = 0;

        if (AICount < 0)
            AICount = 0;

        playerText.text = $"00{playerCount}";
        aIText.text = $"00{AICount}";
    }

    public void InitializeTime(float nowTime)
    {
        getTime = nowTime;

        Debug.Log(getTime);
        int minute = (int)nowTime / 60;
        int second = (int)nowTime % 60;

        timerText.text = $"{minute} : {second}";
    }

    public void UpdateTime()
    {
        if (getTime > 0)
        {
            getTime -= Time.deltaTime;

            int minute = (int)getTime / 60;
            int second = (int)getTime % 60;

            timerText.text = $"{minute} : {second}";
        }
        else
        {
            isGameEnd = true;
            timerText.text = $"{0} : {0}";

            if(!BattleManager.Instance.isC4Install)
                GameManager.Singleton.GameEnd?.Invoke(false);
            else
                GameManager.Singleton.GameEnd?.Invoke(true);
        }
    }
}
