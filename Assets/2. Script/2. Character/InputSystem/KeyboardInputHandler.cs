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

        // Mouse ÀÌº¥Æ®
        public bool IsFirePressed() => Input.GetMouseButtonDown(0);
        public bool IsFireHeld() => Input.GetMouseButton(0);
        public bool IsFireEnd() => Input.GetMouseButtonUp(0);
        public bool IsAimPressed() => Input.GetMouseButtonDown(1);
        public bool IsAimHeld() => Input.GetMouseButton(1);
    }
}