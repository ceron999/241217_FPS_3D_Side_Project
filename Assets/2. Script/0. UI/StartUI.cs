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

    // ���� ����/���� ��ư
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

    // AI �� ����
    public void OnClickPlusButton()
    {
        // 5 �ʰ��� �߰� �Ұ���
        if(aiCount >= 5)
        {
            plusButton.enabled = false;
            return;
        }
        // 1�̸� minus �߰�
        else if(aiCount ==1)
            minusButton.enabled = true;

        aiCount++;
        aICountText.text = aiCount.ToString();
    }

    // AI �� ����
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

    // �� ���� ����
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

    // ���� ����
    public void OnClickStartButton()
    {

    }

    // ���� ����
    public void OnClickEndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
