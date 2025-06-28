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

        // 이벤트
        public event Action OnFire;
        public event Action OnJump;
        public event Action OnReload;

        private void Awake()
        {
            Instance = this;
            inputHandler = new KeyboardInputHandler();
        }

        private void Update()
        {
            if (inputHandler.IsFirePressed())
                OnFire?.Invoke();

            if (inputHandler.IsJumpPressed())
                OnJump?.Invoke();

            if (inputHandler.IsReloadPressed())
                OnReload?.Invoke();
        }
    }
}