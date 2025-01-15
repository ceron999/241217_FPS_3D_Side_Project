using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : StateBase
{
    // Base�� �ִ� �����ڸ� �����϶�� ��
    public BattleState(CharacterController_AI getData) : base(getData) { }

    private CharacterBase linkedAIBase;
    private CharacterBase target;

    public LayerMask targetLayerMask = 1 << 9;      // ĳ���� ���̾�


    public override void OnStateEnter()
    {
        // AI�� ���� -> ���� Ÿ�ٿ��� ���� ������ ��
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
    /// ���� �����ϴ� �Լ�
    /// 1. ���� ���� ���� �ȿ� �����Ƿ� �ٷ� ����
    /// </summary>
    void Battle()
    {
        if (target.IsDie)
            return;

        // ��ǥ ���� ����
        Vector3 pivot = _aiController.transform.position + Vector3.up;
        Vector3 targetPosition = target.transform.position + Vector3.up;
        Vector3 direction = (targetPosition - pivot).normalized;

        // ��ǥ ���⿡ �÷��̾� Ȯ��
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

        // ���� ���� ����
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
