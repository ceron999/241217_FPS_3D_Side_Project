using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeWarningState : StateBase
{
    private Vector3 targetPosition;

    // Base�� �ִ� �����ڸ� �����϶�� ��
    public TakeWarningState(CharacterController_AI getData) : base(getData) { }

    public override void OnStateEnter()
    {
        targetPosition = _aiController.listenPosition;
        _aiController.navMeshAgent.SetDestination(targetPosition);
        Debug.Log("Enter TakeWarning State");
    }
    public override void OnStateUpdate()
    {
        targetPosition = _aiController.listenPosition;
        SearchTargetPosition();

        // C4 ��ġ �� C4 ��ó�� �̵��Ͽ��� �� 
        if(BattleManager.Instance.isC4Install && _aiController.IsFindC4())
        {
            _aiController.UninstallC4();
        }
        else
        {
            _aiController.CancelUninstallC4();
        }
    }
    public override void OnStateExit()
    {
        _aiController.listenPosition = Vector3.zero;
        if(BattleManager.Instance.isC4Install)
            _aiController.CancelUninstallC4();
        Debug.Log("Exit TakeWarning State");
    }

    void SearchTargetPosition()
    {
        if (_aiController.navMeshAgent.hasPath)
        {
            _aiController.SetMoveDirection();

            _aiController.navMeshAgent.SetDestination(targetPosition);
        }
    }
}
