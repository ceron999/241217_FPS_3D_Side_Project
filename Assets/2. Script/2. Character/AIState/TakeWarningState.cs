using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeWarningState : StateBase
{
    private Vector3 targetPosition;

    // Base에 있는 생성자를 실행하라는 뜻
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
    }
    public override void OnStateExit()
    {
        _aiController.listenPosition = Vector3.zero;
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
