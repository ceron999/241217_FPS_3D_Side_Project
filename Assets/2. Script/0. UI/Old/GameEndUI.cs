using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEndUI : UIBase
{
    public static GameEndUI Instance => UIManager.Singleton.GetUI<GameEndUI>(UIList.GameEndUI);

    public TextMeshProUGUI gameEndText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killCountText;
    
    public Button retryButton;
    public Button exitButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(OnClickRetryButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void SetGameEndUI(bool isWin)
    {
        if (isWin)
        {
            gameEndText.text = "Game Win!";
        }
        else
        {
            gameEndText.text = "Game Lose!";
        }

        timerText.text = GameDataUI.Instance.timerText.text;
        killCountText.text = SituationBoardUI.Instance.GetPlayerKillCount(0).ToString();
    }

    public void OnClickRetryButton()
    {
        Main.Singleton.ChangeScene(SceneType.StartScene);
        this.gameObject.SetActive(false);
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
