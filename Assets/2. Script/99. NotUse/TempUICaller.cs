using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUICaller : MonoBehaviour
{
    // 임시로 UI를 부르기 위한 작업
    
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void SetUI()
    {
        UIManager.Show<CrosshairUI>(UIList.CrosshairUI);
        UIManager.Show<RaderUI>(UIList.RaderUI);
        UIManager.Show<GameDataUI>(UIList.GameDataUI);
        UIManager.Show<StatusUI>(UIList.StatusUI);
        UIManager.Show<BulletUI>(UIList.BulletUI);
        UIManager.Show<WeaponUI>(UIList.WeaponUI);
    }
}
