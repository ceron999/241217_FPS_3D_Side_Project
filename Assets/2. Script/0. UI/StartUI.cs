using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MainWeaponType
{
    Rifle,
    Sniper,
}

public class StartUI : UIBase
{
    public static StartUI instance => UIManager.Singleton.GetUI<StartUI>(UIList.StartUI);

    // AI UI
    public int aiCount;
    public TextMeshProUGUI aICountText;
    public Button minusButton;
    public Button plusButton;

    // Weapon UI
    public MainWeaponType mainWeaponType = MainWeaponType.Rifle;
    public TextMeshProUGUI mainWeaponText;
    public Button nextButton;

    // 게임 시작/종료 버튼
    public Button startButton;
    public Button endButton;

    private void Awake()
    {
        aiCount = 1;
        minusButton.enabled = false;
        minusButton.onClick.AddListener(OnClickMinusButton);
        plusButton.onClick.AddListener(OnClickPlusButton);

        nextButton.onClick.AddListener(OnClickNextButton);
        mainWeaponText.text = mainWeaponType.ToString();

        startButton.onClick.AddListener(OnClickStartButton);
        endButton.onClick.AddListener(OnClickEndButton);
    }

    // AI 수 증가
    public void OnClickPlusButton()
    {
        // 5 초과시 추가 불가능
        if(aiCount >= 5)
        {
            plusButton.enabled = false;
            return;
        }
        // 1이면 minus 추가
        else if(aiCount ==1)
            minusButton.enabled = true;

        aiCount++;
        aICountText.text = aiCount.ToString();
    }

    // AI 수 감소
    public void OnClickMinusButton()
    {
        if (aiCount <= 1)
        {
            minusButton.enabled = false;
            return;
        }
        else if (aiCount == 5)
            plusButton.enabled = true;

        aiCount--;
        aICountText.text = aiCount.ToString();
    }

    // 주 무기 변경
    public void OnClickNextButton()
    {
        if(mainWeaponType == MainWeaponType.Rifle)
        {
            mainWeaponType = MainWeaponType.Sniper;
            mainWeaponText.text = mainWeaponType.ToString();
        }
        else
        {
            mainWeaponType = MainWeaponType.Rifle;
            mainWeaponText.text = mainWeaponType.ToString();
        }
    }

    // 게임 시작
    public void OnClickStartButton()
    {

    }

    // 게임 종료
    public void OnClickEndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
