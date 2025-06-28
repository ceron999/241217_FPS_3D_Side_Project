using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        public static InputSystem Instance;

        private IInputHandler inputHandler;

        // 이동관련 변수
        public Vector2 Movement => inputHandler.GetMovement();
        public Vector2 Look => inputHandler.GetLook();
        public bool IsRun => inputHandler.IsRunning();
        public bool IsCrouch => inputHandler.IsCrouching();

        // 마우스 이벤트
        public event Action OnFireStart;
        public event Action OnFireHeld;
        public event Action OnFireEnd;
        public event Action OnAimPressed;
        public event Action OnAimHeld;

        // 키보드 이벤트
        public event Action OnJump;
        public event Action OnReload;
        public event Action OnOpenInventory;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            inputHandler = new KeyboardInputHandler();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            // 마우스 이벤트 확인
            if (inputHandler.IsFirePressed())
                OnFireStart?.Invoke();
            if (inputHandler.IsFireEnd())
                OnFireEnd?.Invoke();
            if (inputHandler.IsAimPressed())
                OnAimPressed?.Invoke();

            if (inputHandler.IsJumpPressed())
                OnJump?.Invoke();

            // 키보드 이벤트 확인
            if (inputHandler.IsReloadPressed())
                OnReload?.Invoke();
        }
    }
}