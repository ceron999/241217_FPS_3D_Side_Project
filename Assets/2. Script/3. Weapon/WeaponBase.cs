using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int RemainAmmo => currentAmmo;

    public Projectile bulletPrefab;
    public Transform firePoint;

    public float fireRate; // ���� �ӵ� (�ð� ��) => ex) 0.1: 0.1�ʿ� 1�߾� �߻� �� �� �ִ� ��
    public int clipSize; // źâ ũ��

    private float lastFireTime; // ������ �߻� ���� �ð�
    public int currentAmmo; // ���� źâ�� ���� �Ѿ� ��

    private void Awake()
    {
        currentAmmo = clipSize;
        fireRate = Mathf.Max(fireRate, 0.1f); // ���� �ӵ��� 0.1���� �۴ٸ�, 0.1�� ����
    }

    public bool Fire()
    {
        if (currentAmmo <= 0 || Time.time - lastFireTime < fireRate)
            return false;

        Projectile bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
        bullet.gameObject.SetActive(true);
        lastFireTime = Time.time;
        currentAmmo--;

        return true;
    }

    public void Reload()
    {
        currentAmmo = clipSize;
    }
}
