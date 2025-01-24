using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class SearchState : StateBase
{
    private Vector3 targetPosition;

    // Base에 있는 생성자를 실행하라는 뜻
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
        // 남은 거리가 얼마 남지 않았다면 다음 위치로 이동하도록 설정
        if (_aiController.IsArrivedDestination())
        {
            Debug.Log($"{_aiController.linkedAIBase.characterIndex} 번째 목표지점 : {_aiController.pointOffset}");
            targetPosition = _aiController.patrolPointList[_aiController.pointOffset];
            _aiController.navMeshAgent.SetDestination(targetPosition);
        }

        // 목표 지점으로 이동
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
