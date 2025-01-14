using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : StateBase
{
    // Base�� �ִ� �����ڸ� �����϶�� ��
    public BattleState(CharacterController_AI getData) : base(getData) { }

    public override void OnStateEnter()
    {
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
}
