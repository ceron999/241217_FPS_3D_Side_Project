using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : WeaponBase
{
    public Projectile bulletPrefab;
    public Transform firePoint;

    public float fireRate; // ���� �ӵ� (�ð� ��) => ex) 0.1: 0.1�ʿ� 1�߾� �߻� �� �� �ִ� ��

    private float lastFireTime; // ������ �߻� ���� �ð�

    private void Awake()
    {
        currentAmmo = clipSize;
        fireRate = Mathf.Max(fireRate, 0.1f); // ���� �ӵ��� 0.1���� �۴ٸ�, 0.1�� ����
    }

    //�� �߻�
    public override bool Activate()
    {
        if (currentAmmo <= 0 || Time.time - lastFireTime < fireRate)
            return false;

        Projectile bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.bulletDamage = weaponDamage;
        bullet.gameObject.SetActive(true);
        lastFireTime = Time.time;
        currentAmmo--;

        // Muzzle effect ���
        EffectManager.Instance.CreateEffect(EffectType.MuzzleFlash1, firePoint.position, firePoint.rotation);

        return true;
    }
}
