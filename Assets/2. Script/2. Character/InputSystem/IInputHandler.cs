using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.InputSystem
{
    public interface IInputHandler
    {
        // 캐릭터 이동 관련
        Vector2 GetMovement();
        Vector2 GetLook();

        bool IsRunning();
        bool IsCrouching();
        bool IsJumpPressed();
        bool IsReloadPressed();
        bool IsFirePressed();
        bool IsFireHeld();
        bool IsAimPressed();

        // UI 관련

    }
}