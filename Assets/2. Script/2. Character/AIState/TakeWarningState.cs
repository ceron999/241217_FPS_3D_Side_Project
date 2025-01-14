using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeWarningState : StateBase
{
    // Base에 있는 생성자를 실행하라는 뜻
    public TakeWarningState(CharacterController_AI getData) : base(getData) { }

    public override void OnStateEnter()
    {
        Debug.Log("Enter TakeWarning State");
    }
    public override void OnStateUpdate()
    {
        Debug.Log("Update TakeWarning State");
    }
    public override void OnStateExit()
    {
        Debug.Log("Exit TakeWarning State");
    }
}
