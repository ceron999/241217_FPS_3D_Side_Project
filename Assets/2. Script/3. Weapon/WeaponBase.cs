using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public float weaponDamage;

    protected int maxAmmo;              // ������ �ִ� ������
    public int holdAmmo;                // ������ ��� ������ ������
    public int RemainAmmo => currentAmmo;
    protected int currentAmmo;         // ���� ��� ���� ������ ���� ����(���� źâ�� ���� ź ����
    protected int clipSize;            // źâ ũ��

    public abstract bool Activate();


    protected virtual void Awake()
    {
        
    }

    public void Reload()
    {
        // ��� ����ؼ� ���� �Ұ�
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
