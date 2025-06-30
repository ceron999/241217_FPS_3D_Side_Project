using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Weapon;

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

        public event Action OnJump;

        // 마우스 이벤트
        public Action OnWeaponStart;
        public Action OnWeaponHeld;
        public Action OnWeaponEnd;
        public Action OnAimPressed;
        // public event Action OnAimEnd;

        // 무기 이벤트
        public event Action<int> OnSwtichWeapon;
        public event Action OnReload;

        // UI 이벤트
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
                OnWeaponStart?.Invoke();
            if (inputHandler.IsFireEnd())
                OnWeaponEnd?.Invoke();
            if (inputHandler.IsAimPressed())
                OnAimPressed?.Invoke();

            if (inputHandler.IsJumpPressed())
                OnJump?.Invoke();

            // 무기 스위칭 이벤트 확인
            if(inputHandler.SwitchWeapon() >=1 && inputHandler.SwitchWeapon() <= 5)
            {
                int switchWeaponNum = inputHandler.SwitchWeapon();
                OnSwtichWeapon?.Invoke(switchWeaponNum);
            }

            // 키보드 이벤트 확인
            if (inputHandler.IsReloadPressed())
                OnReload?.Invoke();
            
            // UI 연동
            if(inputHandler.IsOpenInventory())
                OnOpenInventory?.Invoke();

        }
    }
}