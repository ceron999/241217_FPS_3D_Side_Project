using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class SearchState : StateBase
{
    private Vector3 targetPosition;

    // Base에 있는 생성자를 실행하라는 뜻
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
        // 남은 거리가 얼마 남지 않았다면 다음 위치로 이동하도록 설정
        if (_aiController.IsArrivedDestination())
        {
            Debug.Log($"{_aiController.linkedAIBase.characterIndex} 번째 목표지점 : {_aiController.pointOffset}");
        }

        // 목표 지점으로 이동
        _aiController.SearchPatrolPoints();
    }
    public override void OnStateExit()
    {
        Debug.Log("Exit Search State");
    }
}
