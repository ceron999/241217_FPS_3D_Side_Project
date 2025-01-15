using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : StateBase
{
    // Base에 있는 생성자를 실행하라는 뜻
    public BattleState(CharacterController_AI getData) : base(getData) { }

    private CharacterBase target;


    public override void OnStateEnter()
    {
        target = _aiController.target;
        Debug.Log("Enter Battle State");
    }
    public override void OnStateUpdate()
    {
        Debug.Log("Update Battle State");
    }
    public override void OnStateExit()
    {
        Debug.Log("Exit Battle State");
    }

    /// <summary>
    /// 적을 공격하는 함수
    /// 1. 적이 공격 범위 안에 있으므로 바로 공격
    /// </summary>
    void Battle()
    {

        //float distance = Vector3.Distance(transform.position, target.transform.position);
        //float limitDistance = 10f;
        //if (distance > limitDistance)
        //{
        //    target = null;
        //    linkedCharacter.Shoot(false);
        //    SetAiState(AIState.Peaceful);
        //    return;
        //}

        //float weaponRange = 7f;

        //if (distance > weaponRange)
        //{
        //    ChaseTarget();
        //    UpdateChase();
        //}
        //else
        //{
        //    Vector3 pivot = linkedCharacter.transform.position + Vector3.up;
        //    Vector3 targetPosition = target.transform.position + Vector3.up;
        //    Vector3 direction = (targetPosition - pivot).normalized;

        //    bool isRaycastSuccessToTarget = false;
        //    Ray ray = new Ray(pivot, direction);
        //    if (Physics.Raycast(ray, out RaycastHit hitInfo,
        //        weaponRange, attackValidLayer, QueryTriggerInteraction.Ignore))
        //    {
        //        if (hitInfo.transform.root.gameObject.CompareTag("Player"))
        //        {
        //            isRaycastSuccessToTarget = true;
        //        }
        //    }

        //    Vector3 weaponFirePoint = linkedCharacter.currentWeapon.firePoint.position;
        //    Vector3 directionFromWeapon = (targetPosition - weaponFirePoint).normalized;
        //    bool isRaycastSuccessFromWeapon = false;
        //    Ray weaponRay = new Ray(weaponFirePoint, directionFromWeapon);
        //    if (Physics.Raycast(weaponRay, out RaycastHit weaponHitInfo,
        //        weaponRange, attackValidLayer, QueryTriggerInteraction.Ignore))
        //    {
        //        if (weaponHitInfo.transform.root.gameObject.CompareTag("Player"))
        //        {
        //            isRaycastSuccessFromWeapon = true;
        //        }
        //    }

        //    if (isRaycastSuccessToTarget && isRaycastSuccessFromWeapon)
        //    {
        //        if (target.IsAlive)
        //        {
        //            // TODO : Attack
        //            Transform targetChestTransform = target.GetBoneTransform(HumanBodyBones.Chest);
        //            linkedCharacter.AimingPoint = targetChestTransform.position;
        //            linkedCharacter.transform.forward = (target.transform.position - transform.position).normalized;
        //            linkedCharacter.Move(Vector2.zero, 0);
        //            linkedCharacter.Shoot(true);
        //        }
        //    }
        //    else
        //    {
        //        linkedCharacter.AimingPoint = transform.position + transform.forward * 100f;

        //        ChaseTarget();
        //        UpdateChase();
        //    }
        //}
    }
}
