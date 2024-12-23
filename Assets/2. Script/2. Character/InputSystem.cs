using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player의 조작 관련 Event 모음
/// </summary>

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;

    // 이동
    public Vector2 Movement => movement;
    public Vector2 movement;

    public bool IsRun => isRun;
    private bool isRun = false;
    public bool IsSit => isSit;
    private bool isSit = false;

    // 조작 액션
    public System.Action OnClickAlpha1;                 // 주 무기 스위칭
    public System.Action OnClickAlpha2;                 // 보조 무기 스위칭
    public System.Action OnClickAlpha3;                 // 칼 스위칭
    public System.Action OnClickAlpha4;                 // 수류탄 스위칭
    public System.Action OnClickAlpha5;                 // C4 스위칭

    public System.Action OnClickSpace;                  // 점프

    public System.Action OnClickMouseLeftButtonDown;    // 좌클릭
    public System.Action OnClickMouseRightButton;       // 우클릭

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
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
        float inputY = Input.GetAxis("vertical");
        movement = new Vector2(inputX, inputY);

        // 이동 상태 변환
        isRun = Input.GetKey(KeyCode.LeftShift);
        isSit = Input.GetKey(KeyCode.LeftControl);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickSpace?.Invoke();
        }

    }
}
