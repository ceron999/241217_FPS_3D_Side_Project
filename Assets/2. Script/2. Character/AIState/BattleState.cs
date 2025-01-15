using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : StateBase
{
    // Base에 있는 생성자를 실행하라는 뜻
    public BattleState(CharacterController_AI getData) : base(getData) { }

    private CharacterBase linkedAIBase;
    private CharacterBase target;

    public LayerMask targetLayerMask = 1 << 9;      // 캐릭터 레이어


    public override void OnStateEnter()
    {
        // AI를 정지 -> 이후 타겟에게 방향 설정할 것
        linkedAIBase = _aiController.linkedAIBase;
        target = _aiController.target;

        _aiController.navMeshAgent.SetDestination(_aiController.transform.position);
        _aiController.StopMove();

        linkedAIBase.aimRig.weight = 1;
        Debug.Log("Enter Battle State");
    }
    public override void OnStateUpdate()
    {
        Battle();
    }
    public override void OnStateExit()
    {
        linkedAIBase.aimRig.weight = 0;
        Debug.Log("Exit Battle State");
    }

    /// <summary>
    /// 적을 공격하는 함수
    /// 1. 적이 공격 범위 안에 있으므로 바로 공격
    /// </summary>
    void Battle()
    {
        if (target.IsDie)
            return;

        // 목표 지점 설정
        Vector3 pivot = _aiController.transform.position + Vector3.up;
        Vector3 targetPosition = target.transform.position + Vector3.up;
        Vector3 direction = (targetPosition - pivot).normalized;

        // 목표 방향에 플레이어 확인
        bool isRaycastSuccessToTarget = false;

        Ray ray = new Ray(pivot, direction);
        float maxDistance = 7f;

        // Debug.DrawRay(pivot, direction * 7, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, 
            targetLayerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hitInfo.collider.transform.root.name);
            isRaycastSuccessToTarget = true;
        }

        // 에임 방향 설정
        Debug.DrawRay(pivot, linkedAIBase.transform.forward * 7, Color.red);
        Debug.DrawRay(linkedAIBase.nowWeapon.transform.position,
            linkedAIBase.nowWeapon.transform.forward * 7f, Color.blue);

        linkedAIBase.AimingPoint = targetPosition;

        bool isRaycastSuccessFromWeapon = false;
        Ray weaponRay = new Ray(pivot, linkedAIBase.nowWeapon.transform.forward);
        if (Physics.Raycast(weaponRay, out RaycastHit weaponHitInfo,
            maxDistance, targetLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (weaponHitInfo.transform.root.gameObject.CompareTag("Player"))
            {
                isRaycastSuccessFromWeapon = true;
            }
        }

        if (isRaycastSuccessToTarget && isRaycastSuccessFromWeapon)
        {
            if (!target.IsDie)
            {
                linkedAIBase.Shoot(true);
            }
            else
                linkedAIBase.Shoot(false);
        }

    }
}
