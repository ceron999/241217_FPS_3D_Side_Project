using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DieState : StateBase
{
    // Base�� �ִ� �����ڸ� �����϶�� ��
    public DieState(CharacterController_AI getData) : base(getData) { }

    // Die ���·� ������ 5�� �� �����
    private float deleteTime;
    private float maxDeleteTime = 5f;

    public override void OnStateEnter()
    {
        GameManager.Singleton.PlayerKillAction?.Invoke();
        BattleManager.Instance.AIDie();
        _aiController.navMeshAgent.enabled = false;
        deleteTime = 0f;
        Debug.Log("Enter Die State");
    }
    public override void OnStateUpdate()
    {
        if (deleteTime < maxDeleteTime)
        {
            deleteTime += Time.deltaTime;
        }
        else
            OnStateExit();


    }
    public override void OnStateExit()
    {
        Debug.Log("Exit Die State");
        _aiController.Die();
    }
}
