using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DieState : StateBase
{
    // Base에 있는 생성자를 실행하라는 뜻
    public DieState(CharacterController_AI getData) : base(getData) { }

    // Die 상태로 들어오면 5초 후 사라짐
    private float deleteTime;
    private float maxDeleteTime = 5f;

    public override void OnStateEnter()
    {
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
