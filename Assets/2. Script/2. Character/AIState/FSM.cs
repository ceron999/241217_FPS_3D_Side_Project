using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    private StateBase _curState;

    public FSM(StateBase getState)
    {
        _curState = getState;
    }

    // 상태를 변경
    public void ChangeState(StateBase nextBase)
    {
        if (_curState == nextBase)
            return;

        if (_curState != null)
            _curState.OnStateExit();

        _curState = nextBase;
        _curState.OnStateEnter();
    }

    public void UpdateState()
    {
        if (_curState != null)
            _curState.OnStateUpdate();
    }
}
