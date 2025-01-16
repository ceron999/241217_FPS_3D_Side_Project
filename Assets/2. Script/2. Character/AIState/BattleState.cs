using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : StateBase
{
    // Base�� �ִ� �����ڸ� �����϶�� ��
    public BattleState(CharacterController_AI getData) : base(getData) { }

    public LayerMask targetLayerMask = 1 << 9;      // ĳ���� ���̾�


    public override void OnStateEnter()
    {
        // AI�� ���� -> ���� Ÿ�ٿ��� ���� ������ ��

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
