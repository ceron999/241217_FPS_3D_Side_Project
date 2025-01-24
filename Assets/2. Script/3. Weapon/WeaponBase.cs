using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public float weaponDamage;

    protected int maxAmmo;              // 무기의 최대 보유량
    public int holdAmmo;                // 앞으로 사용 가능한 보유량
    public int RemainAmmo => currentAmmo;
    protected int currentAmmo;         // 현재 사용 중인 무기의 남은 개수(현재 탄창의 남은 탄 개수
    protected int clipSize;            // 탄창 크기

    public abstract bool Activate();


    protected virtual void Awake()
    {
        
    }

    public void Reload()
    {
        // 모두 사용해서 장전 불가
        if (holdAmmo <= 0)
            return;

        if (holdAmmo > clipSize)
        {
            holdAmmo = holdAmmo - clipSize + currentAmmo;
            currentAmmo = clipSize;
        }
        else
        {
            currentAmmo = holdAmmo;
            holdAmmo = 0;
        }

        BulletUI.Instance.UpdateAmmoCount(currentAmmo, holdAmmo);
    }
}
