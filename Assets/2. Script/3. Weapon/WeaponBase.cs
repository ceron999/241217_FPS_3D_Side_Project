using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public bool IsUsable => IsUsable;
    private bool isUsable = true;

    public float weaponDamage;

    public int RemainAmmo => currentAmmo;
    public int currentAmmo;         // ���� ��� ���� ������ ���� ����(���� źâ�� ���� ź ����
    public int clipSize;            // źâ ũ��

    public abstract bool Activate();


    public void Reload()
    {
        currentAmmo = clipSize;
    }

}
