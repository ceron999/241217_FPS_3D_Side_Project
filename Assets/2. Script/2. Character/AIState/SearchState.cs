using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class SearchState : StateBase
{
    private Vector3 targetPosition;

    // Base�� �ִ� �����ڸ� �����϶�� ��
    public SearchState(CharacterController_AI getData) : base(getData) 
    {
        _aiController.target = null;
        targetPosition = _aiController.patrolPointList[_aiController.pointOffset];
        _aiController.navMeshAgent.SetDestination(targetPosition);
    }

    public override void OnStateEnter()
    {
        _aiController.target = null;
        targetPosition = _aiController.patrolPointList[_aiController.pointOffset];
        _aiController.navMeshAgent.SetDestination(targetPosition);
        Debug.Log("Enter Search State");
    }
    public override void OnStateUpdate()
    {
        // ���� �Ÿ��� �� ���� �ʾҴٸ� ���� ��ġ�� �̵��ϵ��� ����
        if (_aiController.IsArrivedDestination())
        {
            Debug.Log($"{_aiController.linkedAIBase.characterIndex} ��° ��ǥ���� : {_aiController.pointOffset}");
            targetPosition = _aiController.patrolPointList[_aiController.pointOffset];
            _aiController.navMeshAgent.SetDestination(targetPosition);
        }

        // ��ǥ �������� �̵�
        SearchPatrolPoints();
    }
    public override void OnStateExit()
    {
        Debug.Log("Exit Search State");
    }

    void SearchPatrolPoints()
    {
        if(_aiController.navMeshAgent.hasPath)
        {
            _aiController.SetMoveDirection();

            _aiController.navMeshAgent.SetDestination(targetPosition);
        }
        else
        {
            targetPosition = _aiController.patrolPointList[_aiController.pointOffset];
            _aiController.navMeshAgent.SetDestination(targetPosition);
        }
    }
}
