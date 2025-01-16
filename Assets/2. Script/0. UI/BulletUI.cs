using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletUI : UIBase
{
    public static BulletUI Instance => UIManager.Singleton.GetUI<BulletUI>(UIList.BulletUI);

    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI bulletText;

    public void InitializeBulletUI(WeaponBase getWeaponData)
    {
        weaponText.text = getWeaponData.name;

        bulletText.text = $"{getWeaponData.RemainAmmo} / {getWeaponData.holdAmmo}";
    }

    public void ChangeWeapon(WeaponBase getWeaponData)
    {
        weaponText.text = getWeaponData.name;

        bulletText.text = $"{getWeaponData.RemainAmmo} / {getWeaponData.holdAmmo}";
    }

    public void UpdateAmmoCount(int remainAmmo, int holdAmmo)
    {
        bulletText.text = $"{remainAmmo} / {holdAmmo}";
    }
}
