using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    // 무기 데이터
    public List<WeaponBase> weapons;

    // 달리기 및 앉기 관련 데이터
    
    private void Update()
    {
        // 1. 지형 확인
        CheckGround();

        // 2. 사격
        if(isShooting)
        {
            bool isFireSuccess = nowWeapon.Fire();
            if (false == isFireSuccess)
            {
                if (nowWeapon.RemainAmmo <= 0 && false == isReloading)
                {
                    isReloading = true;
                    Reload();
                }
            }
            nowWeapon.Fire();
        }

        // 3. animation 설정
        
        runningBlend = Mathf.Lerp(runningBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);
        crouchBlend = Mathf.Lerp(crouchBlend, IsCrouch ? 1f : 0f, Time.deltaTime * 10f);

        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetFloat("Horizontal", horizontal);
        characterAnimator.SetFloat("Vertical", vertical);
        characterAnimator.SetFloat("RunningBlend", runningBlend);
        characterAnimator.SetFloat("CrouchBlend", crouchBlend);
    }

    // 무기 스위칭 관련 데이터
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
}
