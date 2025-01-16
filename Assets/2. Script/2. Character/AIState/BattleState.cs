using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : StateBase
{
    // Base에 있는 생성자를 실행하라는 뜻
    public BattleState(CharacterController_AI getData) : base(getData) { }

    public LayerMask targetLayerMask = 1 << 9;      // 캐릭터 레이어


    public override void OnStateEnter()
    {
        // AI를 정지 -> 이후 타겟에게 방향 설정할 것

        _aiController.navMeshAgent.SetDestination(_aiController.transform.position);
        _aiController.StopMove();

        _aiController.SetAimRigWeight(1);
        Debug.Log("Enter Battle State");
    }
    public override void OnStateUpdate()
    {
        _aiController.Battle();
    }
    public override void OnStateExit()
    {
        _aiController.SetAimRigWeight(0);
        Debug.Log("Exit Battle State");
    }
}
