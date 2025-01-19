using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : UIBase
{
    public static WeaponUI Instance => UIManager.Singleton.GetUI<WeaponUI>(UIList.WeaponUI);

    public override void Show()
    {
        base.Show();

        StartCoroutine(WeaponUIShowCoroutine());
    }

    public GameObject grenadeUI;

    // 3ÃÊ µÚ¿¡ ²¨Áü
    IEnumerator WeaponUIShowCoroutine()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }

    public void SetGrenadeUI()
    {
        grenadeUI.SetActive(false);
    }
}
