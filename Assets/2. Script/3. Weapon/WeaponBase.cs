using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Weapon;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;

    [Header("무기 데이터")]
    public float weaponDamage;

    [Header("탄창 보유량")]
    protected int maxAmmo;              // 무기의 최대 보유량
    public int holdAmmo;                // 앞으로 사용 가능한 보유량
    public int RemainAmmo => remainAmmo;
    protected int remainAmmo;           // 현재 사용 중인 무기의 남은 개수(현재 탄창의 남은 탄 개수
    protected int clipSize;             // 탄창 크기

    public void Initialize()
    {
        weaponDamage = _weaponData.weaponDamage;

        maxAmmo = _weaponData.maxAmmo;
        holdAmmo = _weaponData.maxAmmo;
        remainAmmo = _weaponData.maxAmmo;
        clipSize = _weaponData.maxAmmo;   
    }

    public abstract bool Activate();


    protected virtual void Awake()
    {
        Initialize();
    }

    public void Reload()
    {
        // 모두 사용해서 장전 불가
        if (holdAmmo <= 0)
            return;

        if (holdAmmo > clipSize)
        {
            holdAmmo = holdAmmo - clipSize + remainAmmo;
            remainAmmo = clipSize;
        }
        else
        {
            remainAmmo = holdAmmo;
            holdAmmo = 0;
        }
    }
}
