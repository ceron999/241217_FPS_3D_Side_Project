using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    protected CharacterController_AI _aiController;

    protected StateBase(CharacterController_AI getData)
    {
        _aiController = getData;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}
