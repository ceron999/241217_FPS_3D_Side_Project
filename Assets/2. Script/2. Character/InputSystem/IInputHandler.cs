using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.InputSystem
{
    public interface IInputHandler
    {
        // 캐릭터 기본 이동 관련
        Vector2 GetMovement();
        Vector2 GetLook();
        bool IsRunning();
        bool IsCrouching();

        // 캐릭터 특수 이동 관련
        bool IsJumpPressed();
        bool IsReloadPressed();

        // 마우스 Action
        bool IsFirePressed();
        bool IsFireHeld();
        bool IsFireEnd();
        bool IsAimPressed();
        bool IsAimEnd();

        // 무기 관련
        int SwitchWeapon();

        // UI 관련
        bool IsOpenInventory();
        bool IsCloseInventory();
    }
}