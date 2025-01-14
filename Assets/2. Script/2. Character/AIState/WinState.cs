using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : StateBase
{
    // Base에 있는 생성자를 실행하라는 뜻
    public WinState(CharacterController_AI getData) : base(getData) { }

    public override void OnStateEnter()
    {
        Debug.Log("Enter Win State");
    }
    public override void OnStateUpdate()
    {
        Debug.Log("Update Win State");
    }
    public override void OnStateExit()
    {
        Debug.Log("Exit Win State");
    }
}
