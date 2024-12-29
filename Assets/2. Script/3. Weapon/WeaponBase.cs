using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public bool IsUsable => IsUsable;
    private bool isUsable = true;

    public float weaponDamage;

    public int RemainAmmo => currentAmmo;
    public int currentAmmo;         // 현재 사용 중인 무기의 남은 개수(현재 탄창의 남은 탄 개수
    public int clipSize;            // 탄창 크기

    public abstract bool Activate();


    public void Reload()
    {
        currentAmmo = clipSize;
    }

}
