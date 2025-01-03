using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    // ���� ������
    public List<WeaponBase> weapons;

    //��ô ������
    public Transform throwPivot;
    public GameObject throwPrefab;

    private void Update()
    {
        // 1. ���� Ȯ��
        CheckGround();
        
        // 3. animation ����

        runningBlend = Mathf.Lerp(runningBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);
        crouchBlend = Mathf.Lerp(crouchBlend, IsCrouch ? 1f : 0f, Time.deltaTime * 10f);

        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetFloat("Horizontal", horizontal);
        characterAnimator.SetFloat("Vertical", vertical);
        characterAnimator.SetFloat("RunningBlend", runningBlend);
        characterAnimator.SetFloat("CrouchBlend", crouchBlend);
    }

    private void LateUpdate()
    {
        // 2. ���
        /// ����� ��� �Ѿ��� �ұ�Ģ�ϰ� �ٸ� �������� Ƣ�� ������ �־���.
        /// �ش� ������ �� �ڵ尡 update���� lateUpdate�� �̵��ϴϱ� ���� �ذ�
        /// �ش� ������ Animation���� ���� ��ġ�� �ٲ�鼭 �Ѿ˵� �׸� ���� ������ ����
        if (isShooting)
        {
            bool isFireSuccess = nowWeapon.Activate();
            if (false == isFireSuccess)
            {
                if (nowWeapon.holdAmmo <= 0)
                    return;

                if (nowWeapon.RemainAmmo <= 0 && false == isReloading)
                {
                    isReloading = true;
                    Reload();
                }
            }
            //nowWeapon.Activate();
        }

    }

    // ���� ����Ī ���� ������
    public void SwitchMainWeapon()
    {
        nowWeapon = weapons[0];
        SetWeaponActive(0);
    }

    public void SwitchPistol()
    {
        nowWeapon = weapons[1];
        SetWeaponActive(1);
    }

    public void SwitchKnife()
    {
        nowWeapon = weapons[2];
        SetWeaponActive(2);
    }

    public void SwitchGrenade()
    {
        nowWeapon = weapons[3];
        SetWeaponActive(3);
    }

    public void SwitchC4()
    {
        nowWeapon = weapons[4];
        SetWeaponActive(4);
    }

    void SetWeaponActive(int weaponIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i != weaponIndex)
                weapons[i].gameObject.SetActive(false);
            else
                weapons[i].gameObject.SetActive(true);
        }
    }

    public void ThrowStart()
    {
        characterAnimator.SetTrigger("Throw Start Trigger");
    }

    public void SetGrenadeVisual(int active)
    {
        bool isActive = active == 1 ? true : false;
        nowWeapon.gameObject.SetActive(isActive);
    }

    public void ThrowEnd()
    {
        characterAnimator.SetTrigger("Throw End Trigger");
    }
}
