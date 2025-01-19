using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    // 무기 데이터
    public List<WeaponBase> weapons;

    private void Start()
    {
        StatusUI.Instance.SetHP(curStat.HP);

        if (GameManager.StartData.startMainWeaponType == MainWeaponType.Rifle)
        {
            Destroy(weapons[1].gameObject);
            weapons.RemoveAt(1);
        }
        else
        {
            Destroy(weapons[0].gameObject);
            weapons.RemoveAt(0);
        }
        
        nowWeapon = weapons[0];
        BulletUI.Instance.InitializeBulletUI(nowWeapon);
    }

    private void Update()
    { 
        // 1. 지형 확인
        CheckGround();

        // 2. 죽었으면 아무것도 못건들게
        if (IsDie)
            return;

        // 3. animation 설정

        runningBlend = Mathf.Lerp(runningBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);
        crouchBlend = Mathf.Lerp(crouchBlend, IsCrouch ? 1f : 0f, Time.deltaTime * 10f);

        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetFloat("Horizontal", horizontal);
        characterAnimator.SetFloat("Vertical", vertical);
        characterAnimator.SetFloat("RunningBlend", runningBlend);
        characterAnimator.SetFloat("CrouchBlend", crouchBlend);

        // 4. 소리 설정
        audioSource.volume = IsCrouch ? 0f : 1f;
    }

    private void LateUpdate()
    {
        // 1. 죽었으면 아무것도 못건들게
        if (IsDie)
            return;

        // 2. 사격
        /// 사격할 경우 총알이 불규칙하게 다른 방향으로 튀는 문제가 있었음.
        /// 해당 문제는 이 코드가 update에서 lateUpdate로 이동하니까 문제 해결
        /// 해당 문제는 Animation에서 뭔가 위치가 바뀌면서 총알도 그리 나간 것으로 보임
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

    // 무기 스위칭 관련 데이터
    public void SwitchMainWeapon()
    {
        aimRig.weight = 1f;
        nowWeapon = weapons[0];
        SetWeaponActive(0);
    }

    public void SwitchPistol()
    {
        aimRig.weight = 1f;
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
        aimRig.weight = 0f;
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

    public override void ApplyDamage(float getDamage)
    {
        base.ApplyDamage(getDamage);

        if(curStat.HP >= 0)
            StatusUI.Instance.SetHP(curStat.HP);
        else
            StatusUI.Instance.SetHP(0);

        // 사망 UI에게 전송
        if(IsDie)
            BattleManager.Instance.PlayerDie();
    }
}
