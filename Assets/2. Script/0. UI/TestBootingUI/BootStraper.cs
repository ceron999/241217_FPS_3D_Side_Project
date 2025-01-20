using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���߿�
/// ���� ������ ������ �� �⺻���� ��� �����ϴ� Ŭ����
/// </summary>
public class BootStraper : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void SystemBoot()
    {
        if(SceneManager.GetActiveScene().name == "GameScene")
            InternalBoot();
    }

    private static void InternalBoot()
    {
        // 1. ���� �� UI �ҷ�����
        UIManager.Show<CrosshairUI>(UIList.CrosshairUI);
        UIManager.Show<RaderUI>(UIList.RaderUI);
        UIManager.Show<GameDataUI>(UIList.GameDataUI);
        UIManager.Show<StatusUI>(UIList.StatusUI);
        UIManager.Show<BulletUI>(UIList.BulletUI);
        UIManager.Show<WeaponUI>(UIList.WeaponUI);

        // 2. �⺻ �ý��� ����
        GameManager.Singleton.SetStartData();
        GameManager.Singleton.GameStart?.Invoke();

    }
}
