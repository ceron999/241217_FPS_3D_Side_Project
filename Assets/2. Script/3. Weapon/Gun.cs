using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : WeaponBase
{
    public Projectile bulletPrefab;
    public Transform firePoint;

    public float fireRate; // 연사 속도 (시간 값) => ex) 0.1: 0.1초에 1발씩 발사 할 수 있는 값

    private float lastFireTime; // 마지막 발사 실제 시간

    private void Awake()
    {
        currentAmmo = clipSize;
        fireRate = Mathf.Max(fireRate, 0.1f); // 연사 속도가 0.1보다 작다면, 0.1로 설정
    }

    //총 발사
    public override bool Activate()
    {
        if (currentAmmo <= 0 || Time.time - lastFireTime < fireRate)
            return false;

        Projectile bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.bulletDamage = weaponDamage;
        bullet.gameObject.SetActive(true);
        lastFireTime = Time.time;
        currentAmmo--;

        // Muzzle effect 출력
        EffectManager.Instance.CreateEffect(EffectType.MuzzleFlash1, firePoint.position, firePoint.rotation);

        return true;
    }
}
