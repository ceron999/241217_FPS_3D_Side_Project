using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class SearchState : StateBase
{
    private Vector3 targetPosition;

    // Base�� �ִ� �����ڸ� �����϶�� ��
    public SearchState(CharacterController_AI getData) : base(getData) 
    {
        _aiController.target = null;
        _aiController.navMeshAgent.SetDestination(_aiController.patrolPointList[_aiController.pointOffset]);
    }

    public override void OnStateEnter()
    {
        _aiController.target = null;
        _aiController.navMeshAgent.SetDestination(_aiController.patrolPointList[_aiController.pointOffset]);
        Debug.Log("Enter Search State");
    }
    public override void OnStateUpdate()
    {
        // ���� �Ÿ��� �� ���� �ʾҴٸ� ���� ��ġ�� �̵��ϵ��� ����
        if (_aiController.IsArrivedDestination())
        {
            Debug.Log($"{_aiController.linkedAIBase.characterIndex} ��° ��ǥ���� : {_aiController.pointOffset}");
        }

        // ��ǥ �������� �̵�
        _aiController.SearchPatrolPoints();
    }
    public override void OnStateExit()
    {
        Debug.Log("Exit Search State");
    }
}
