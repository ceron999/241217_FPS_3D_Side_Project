using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUICaller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Show<RaderUI>(UIList.RaderUI);
        UIManager.Show<GameDataUI>(UIList.GameDataUI);
        UIManager.Show<StatusUI>(UIList.StatusUI);
        UIManager.Show<BulletUI>(UIList.BulletUI);


        UIManager.Show<WeaponUI>(UIList.WeaponUI);
        UIManager.Hide<SummaryBoardUI>(UIList.SummaryBoardUI);
    }
}
