using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : StateBase
{
    // Base에 있는 생성자를 실행하라는 뜻
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
