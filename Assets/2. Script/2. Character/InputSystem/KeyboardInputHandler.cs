using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.InputSystem
{
    public class KeyboardInputHandler : IInputHandler
    {
        public Vector2 GetMovement() => new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        public Vector2 GetLook() => new(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        public bool IsRunning() => Input.GetKey(KeyCode.LeftShift);
        public bool IsCrouching() => Input.GetKey(KeyCode.LeftControl);


        public bool IsJumpPressed() => Input.GetKeyDown(KeyCode.Space);
        public bool IsReloadPressed() => Input.GetKeyDown(KeyCode.R);

        // Mouse 이벤트
        public bool IsFirePressed() => Input.GetMouseButtonDown(0);
        public bool IsFireHeld() => Input.GetMouseButton(0);
        public bool IsFireEnd() => Input.GetMouseButtonUp(0);
        public bool IsAimPressed() => Input.GetMouseButtonDown(1);
        public bool IsAimEnd() => Input.GetMouseButtonUp(1);


        // 무기 관련
        public int SwitchWeapon()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
            else if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;
            else if (Input.GetKeyDown(KeyCode.Alpha5)) return 5;
            else
            {
                return -1;
            }
        }

        public bool IsOpenInventory() => Input.GetKeyDown(KeyCode.Tab);
    }
}