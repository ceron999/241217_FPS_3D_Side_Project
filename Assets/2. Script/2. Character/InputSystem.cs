using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player의 조작 관련 Event 모음
/// </summary>

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;

    // 커서
    public bool isStopCameraMove = false;
    private bool isShowCursor = false;

    // 이동
    public Vector2 Movement => movement;
    private Vector2 movement;

    public bool IsRun => isRun;
    private bool isRun = false;
    public bool IsCrouch => isCrouch;
    private bool isCrouch = false;

    // 회전
    public Vector2 Look => look;
    private Vector2 look;

    // 조작 액션
    public System.Action OnClickAlpha1;                 // 주 무기 스위칭
    public System.Action OnClickAlpha2;                 // 보조 무기 스위칭
    public System.Action OnClickAlpha3;                 // 칼 스위칭
    public System.Action OnClickAlpha4;                 // 수류탄 스위칭
    public System.Action OnClickAlpha5;                 // C4 스위칭

    public System.Action OnClickSpace;                  // 점프

    public System.Action OnClickLeftMouseButtonDown;    // 좌클릭 down
    public System.Action OnClickLeftMouseButton;        // 좌클릭 누른 상태
    public System.Action OnClickLeftMouseButtonUp;      // 좌클릭 up
    public System.Action OnClickRightMouseButtonDown;   // 우클릭

    public System.Action OnClickR;                      // 재장전

    // UI 관련
    public System.Action OnClickTabDown;                    // 상황판 표시
    public System.Action OnClickTabUp;                    // 상황판 끄기

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // 무기 변환
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnClickAlpha1?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnClickAlpha2?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnClickAlpha3?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnClickAlpha4?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnClickAlpha5?.Invoke();
        }

        // 이동 설정
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        movement = new Vector2(inputX, inputY);

        // 이동 상태 변환
        isRun = Input.GetKey(KeyCode.LeftShift);
        isCrouch = Input.GetKey(KeyCode.LeftControl);

        // 회전 상태 변환
        if (!isStopCameraMove)
        {
            float lookX = Input.GetAxis("Mouse X");
            float lookY = Input.GetAxis("Mouse Y");
            look = isShowCursor ? Vector2.zero : new Vector2(lookX, lookY * (-1f));
        }
        else
        {
            look = Vector2.zero;
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickSpace?.Invoke();
        }

        // 재장전
        if(Input.GetKeyDown(KeyCode.R))
        {
            OnClickR?.Invoke();
        }

        // 공격
        if (Input.GetMouseButtonDown(0))
        {
            OnClickLeftMouseButtonDown?.Invoke();
        }

        if (Input.GetMouseButton(0))
        {
            OnClickLeftMouseButton?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnClickLeftMouseButtonUp?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnClickRightMouseButtonDown?.Invoke();
        }

        // UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnClickTabDown?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            OnClickTabUp?.Invoke();
        }
    }

    public void Clear()
    {
        // 조작 액션
        OnClickAlpha1 = null;                 
        OnClickAlpha2 = null;
        OnClickAlpha3 = null;
        OnClickAlpha4 = null;
        OnClickAlpha5 = null;

        OnClickR = null;

        OnClickAlpha3 = null;

        OnClickLeftMouseButtonDown = null;
        OnClickLeftMouseButtonUp = null;
        OnClickRightMouseButtonDown = null;

    }
}
