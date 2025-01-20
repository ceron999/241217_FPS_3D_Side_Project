using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 개발용
/// 게임 씬에서 부팅할 때 기본적인 기능 설정하는 클래스
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
        // 1. 게임 씬 UI 불러오기
        UIManager.Show<CrosshairUI>(UIList.CrosshairUI);
        UIManager.Show<RaderUI>(UIList.RaderUI);
        UIManager.Show<GameDataUI>(UIList.GameDataUI);
        UIManager.Show<StatusUI>(UIList.StatusUI);
        UIManager.Show<BulletUI>(UIList.BulletUI);
        UIManager.Show<WeaponUI>(UIList.WeaponUI);

        // 2. 기본 시스템 설정
        GameManager.Singleton.SetStartData();
        GameManager.Singleton.GameStart?.Invoke();

    }
}
