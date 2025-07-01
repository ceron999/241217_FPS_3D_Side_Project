using System.Collections;
using System.Collections.Generic;
using UI;
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
        if(SceneManager.GetActiveScene().name == "TestScene")
            InternalBoot();
    }

    private static void InternalBoot()
    {
        // 1. 게임 씬 UI 불러오기
        UIManager.Show<CrosshairUI>(UIList.CrosshairUI);
        UIManager.Hide<InventoryUI>(UIList.InventoryUI);
    }
}
