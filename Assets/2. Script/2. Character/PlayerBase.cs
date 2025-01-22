using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    // ���� ������
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
        // 1. ���� Ȯ��
        CheckGround();

        // 2. �׾����� �ƹ��͵� ���ǵ��
        if (IsDie)
            return;

        // 3. animation ����

        runningBlend = Mathf.Lerp(runningBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);
        crouchBlend = Mathf.Lerp(crouchBlend, IsCrouch ? 1f : 0f, Time.deltaTime * 10f);

        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetFloat("Horizontal", horizontal);
        characterAnimator.SetFloat("Vertical", vertical);
        characterAnimator.SetFloat("RunningBlend", runningBlend);
        characterAnimator.SetFloat("CrouchBlend", crouchBlend);

        // ĳ���� ���� �ִϸ��̼� ����


        // 4. �Ҹ� ����
        audioSource.volume = IsCrouch ? 0f : 1f;
    }

    private void LateUpdate()
    {
        // 1. �׾����� �ƹ��͵� ���ǵ��
        if (IsDie)
            return;

        // 2. ���
        /// ����� ��� �Ѿ��� �ұ�Ģ�ϰ� �ٸ� �������� Ƣ�� ������ �־���.
        /// �ش� ������ �� �ڵ尡 update���� lateUpdate�� �̵��ϴϱ� ���� �ذ�
        /// �ش� ������ Animation���� ���� ��ġ�� �ٲ�鼭 �Ѿ˵� �׸� ���� ������ ����
        if (isShooting && !GameManager.Singleton.isGameEnd)
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
        }

    }

    // ���� ����Ī ���� ������
    public void SwitchMainWeapon()
    {
        characterAnimator.SetFloat("RifleBlend", 1);
        aimRig.weight = 1f;
        nowWeapon = weapons[0];
        SetWeaponActive(0);
    }

    public void SwitchPistol()
    {
        characterAnimator.SetFloat("RifleBlend", 0);
        aimRig.weight = 1f;
        nowWeapon = weapons[1];
        SetWeaponActive(1);
    }

    public void SwitchGrenade()
    {
        // �⺻ �ִϸ��̼� ���·� ����
        characterAnimator.SetFloat("RifleBlend", 1);
        aimRig.weight = 0f;
        nowWeapon = weapons[2];
        SetWeaponActive(2);
    }

    public void SwitchC4()
    {
        characterAnimator.SetFloat("RifleBlend", 1);
        nowWeapon = weapons[3];
        SetWeaponActive(3);
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

        // ��� UI���� ����
        if (IsDie)
        {
            GameManager.Singleton.PlayerDieAction?.Invoke();
            BattleManager.Instance.PlayerDie();
        }
    }
}
