using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : StateBase
{
    // Base�� �ִ� �����ڸ� �����϶�� ��
    public DieState(CharacterController_AI getData) : base(getData) { }

    public override void OnStateEnter()
    {
        Debug.Log("Enter Die State");
    }
    public override void OnStateUpdate()
    {
        Debug.Log("Update Die State");
    }
    public override void OnStateExit()
    {
        Debug.Log("Exit Die State");
    }
}
