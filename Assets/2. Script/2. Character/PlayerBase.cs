using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

public class PlayerBase : CharacterBase
{
    // 무기 데이터
    public WeaponInventory _weaponInventory;

    [Header("인벤토리 관련 변수")]
    private const float checkWeaponRadius = 3f;
    [SerializeField] private LayerMask weaponLayer;

    protected override void Awake()
    {
        base.Awake();
        _weaponInventory = GetComponent<WeaponInventory>();
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

        // 근처 무기 확인
        CheckSurroundingItems();

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
        if (isShooting && !GameManager.Singleton.isGameEnd)
        {
            bool isFireSuccess = _weaponInventory.CurrWeapon.Activate();
            if (false == isFireSuccess)
            {
                if (_weaponInventory.CurrWeapon.holdAmmo <= 0)
                    return;

                if (_weaponInventory.CurrWeapon.RemainAmmo <= 0 && false == isReloading)
                {
                    isReloading = true;
                    Reload();
                }
            }
        }

    }

    // 무기 스위칭할 때 애니메이션 및 모습 정보 변경
    public void SetPlayerAnimationAndRIg(AnimVariables animVariables)
    {
        characterAnimator.SetFloat("RifleBlend", animVariables.rifleBlend);
        aimRig.weight = animVariables.rigWeight;

        leftHand_IK_Target.localPosition = animVariables.weapon_LeftHandIk_Target_Position;
        leftHand_IK_Target.localRotation = Quaternion.Euler(animVariables.weapon_LeftHandIk_Target_Rotation);
    }

    public void SwitchWeapon(int inputIndex)
    {
        WeaponSlot currWeaponSlot = _weaponInventory.SwitchWeapon(inputIndex);
        AnimVariables animVariables = currWeaponSlot.GetAnimVariables();
        SetPlayerAnimationAndRIg(animVariables);
    }

    public void ThrowStart()
    {
        characterAnimator.SetTrigger("Throw Start Trigger");
    }

    public void ThrowEnd()
    {
        characterAnimator.SetTrigger("Throw End Trigger");
    }

    public override void ApplyDamage(float getDamage)
    {
        base.ApplyDamage(getDamage);
    }

    private void CheckSurroundingItems()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, checkWeaponRadius, weaponLayer);

        int minColliderIndex = 0;
        float minDistance = int.MaxValue;

        // 범위 안에서 에임을 둔 무기가 1순위
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, weaponLayer, QueryTriggerInteraction.Ignore))
        {
            // 범위 안에 들어오고 weaponslot을 가지고 있으면 반환
            if ((hitInfo.transform.position - transform.position).sqrMagnitude < checkWeaponRadius * checkWeaponRadius)
            {
                if (hitInfo.collider.TryGetComponent<WeaponSlot>(out WeaponSlot raycastWeapon))
                {
                    raycastWeapon.CheckWeapon();
                    return;
                }
            }
            
        }

        // 이후 가까운 순서대로 2순위
        for (int i = 0; i < colliders.Length; i++)
        {
            float sqrDist = (colliders[i].transform.position - transform.position).sqrMagnitude;
            if (sqrDist < minDistance)
            {
                minColliderIndex = i;
                minDistance = sqrDist;
            }
        }

        if (colliders[minColliderIndex].TryGetComponent<WeaponSlot>(out WeaponSlot minDistWeapon))
        {
            minDistWeapon.CheckWeapon();
        }
    }
}
