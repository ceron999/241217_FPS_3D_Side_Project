using Character.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// class 요약: player의 조작 관련 함수 체인 및 구체화
/// </summary>

public class CharacterController : MonoBehaviour
{
    public PlayerBase player;

    #region 투사 변수
    [Header("투사 데이터")]
    public Transform throwStartPivot;
    public GameObject throwObjectPrefab;
    private bool isThrowMode = false;
    private bool isThrowEnd = false;

    public float throwPower = 10f;

    [Header("투사 궤적 데이터")]
    public LineRenderer grenadeLineRenderer;
    public int linePoint;
    #endregion

    #region 카메라 변수
    [Header("카메라 데이터")]
    public Transform cameraPivot;
    public float bottomClamp = -90f;
    public float topClamp = 90f;
    private float targetYaw;
    private float targetpitch;
    #endregion

    #region C4 설치 변수
    [Header("C4 데이터")]
    public Transform installPosition;
    public float installRadius = 3f; 
    #endregion

    public bool isZoom  = false;

    private void Awake()
    {
        player = GetComponent<PlayerBase>();
    }

    private void Start()
    {
        InputSystem.Instance.OnWeaponStart += CommandFireStart;
        InputSystem.Instance.OnWeaponEnd += CommandFireStop;

        // 무기 스위칭 연결
        InputSystem.Instance.OnSwtichWeapon += CommandSwitchWeapon;

        InputSystem.Instance.OnReload += CommandReload;

        CameraSystem.Instance.SetCameraFollowTarget(cameraPivot);
    }

    private void Update()
    {
        if(!player.IsDie)
        {
            player.SetRunning(InputSystem.Instance.IsRun);
            if (Input.GetKey(KeyCode.LeftShift))
                player.aimRig.weight = 0f;
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                player.aimRig.weight = 1f;
            player.SetCrouch(InputSystem.Instance.IsCrouch);
            player.Move(InputSystem.Instance.Movement);

            player.Rotate(InputSystem.Instance.Look.x);
            player.AimingPoint = CameraSystem.Instance.AimingPoint;
        }
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (InputSystem.Instance.Look.magnitude > 0f)
        {
            float yaw = InputSystem.Instance.Look.x;
            float pitch = InputSystem.Instance.Look.y;

            targetYaw += yaw;
            targetpitch += pitch;
        }

        targetYaw = ClampAngle(targetYaw, float.MinValue, float.MaxValue);
        targetpitch = ClampAngle(targetpitch, bottomClamp, topClamp);
        cameraPivot.rotation = Quaternion.Euler(targetpitch, targetYaw, 0f);
    }

    private float ClampAngle(float IfAngle, float IfMin, float IfMax)
    {
        if (IfAngle < -360f) IfAngle += 360f;
        if (IfAngle > 360f) IfAngle -= 360f;
        return Mathf.Clamp(IfAngle, IfMin, IfMax);
    }

    #region Command Function
    void CommandJump()
    {
        //player.Jump();
    }

    // 총
    void CommandFireStart()
    {
        player.Shoot(true);
    }

    void CommandFireStop()
    {
        player.Shoot(false);
    }

    void CommandReload()
    {
        player.Reload();
    }

    void CommandZoomIn()
    {
        isZoom = !isZoom;
        if (isZoom)
        {
            CameraSystem.Instance.SetActiveScopeMode(true);
            ScopeVinetteController.Instance.SetActiveVinette(true);
        }
        else
        {
            CameraSystem.Instance.SetActiveScopeMode(false);
            ScopeVinetteController.Instance.SetActiveVinette(false);
        }
    }

    // 수류탄

    /// <summary>
    /// 좌클릭을 누르면 시작
    /// 1. 던지기 모션 시작
    /// 2. 누른 위치에서 예상 투척 라인 표시
    /// </summary>
    void CommandThrowStart()
    {
        if (isThrowEnd)
            return;

        if (!isThrowMode)
        {
            player.aimRig.weight = 0;
            isThrowMode = true;
            player.ThrowStart();
        }
    }

    /// <summary>
    /// 애니메이션 중간에 적용되는 함수
    /// 투사체를 생성하여 던짐
    /// </summary>
    public void ThrowPrefab()
    {
        GameObject throwObject = Instantiate(throwObjectPrefab, throwStartPivot);
        if(throwObject.TryGetComponent<Grenade>(out Grenade grenadeComponent))
        {
            // 투척 시작 지점, 투척 힘 지정 
            grenadeComponent.throwStartPivot = throwStartPivot;
            grenadeComponent.throwPower = throwPower;

            // 투척 방향 지정
            Vector3 throwDirection = player.aimingPointTransform.position - throwStartPivot.position;
            throwDirection.y = 0;
            grenadeComponent.throwVector = throwDirection.normalized + Vector3.up;
            grenadeComponent.throwPower = throwPower;
            grenadeComponent.Activate();

            //WeaponUI.Instance.SetGrenadeUIOff();
            //OldInputSystem.Instance.OnClickAlpha3 = null;
        }
    }

    private void CalculateThrowLine()
    {
        grenadeLineRenderer.positionCount = 0;
        grenadeLineRenderer.positionCount = linePoint;

        for (int i = 0; i< linePoint; i++)
        {

        }
    }

    void CommandThrowEnd()
    {
        if (isThrowMode)
        {
            player.ThrowEnd();
            isThrowMode = false;
            isThrowEnd = true;
        }
    }

    public void CommandInstallC4()
    {
        
    }

    // 무기 변환
    private void CommandSwitchWeapon(int inputIndex)
    {
        player.SwitchWeapon(inputIndex);

        // input을 초기화하고 무기 전용 input으로 변경
        ClearMouseInput();

        switch(inputIndex)
        {
            case 1:
                SetGunInputs();
                break;
            case 2:
                SetGunInputs();
                InputSystem.Instance.OnAimPressed += CommandZoomIn;
                break;
            case 3:
                SetGunInputs();
                break;
            case 4:
                SetGrenadeInputs();
                break;
            case 5:
                SetC4Inputs();
                break;
        }
    }

    private void ClearMouseInput()
    {
        InputSystem.Instance.OnWeaponStart = null;
        InputSystem.Instance.OnWeaponHeld = null;
        InputSystem.Instance.OnWeaponEnd = null;

        InputSystem.Instance.OnAimPressed = null;
    }


    public void SetGunInputs()
    {
        InputSystem.Instance.OnWeaponStart += CommandFireStart;
        InputSystem.Instance.OnWeaponEnd += CommandFireStop;
    }
    public void SetGrenadeVisual()
    {

    }

    public void SetGrenadeInputs()
    {
        // 수류탄을 사용했다면 스위칭 못하도록
        if (isThrowEnd)
            return;

        InputSystem.Instance.OnWeaponStart += CommandThrowStart;
        InputSystem.Instance.OnWeaponEnd += CommandThrowEnd;
    }

    public void SetC4Inputs()
    {
        InputSystem.Instance.OnWeaponStart += CommandInstallC4;
    }


    // 상황판 표시 및 끄기
    public void CommandSummaryBoardOpen()
    {
        UIManager.Show<SituationBoardUI>(UIList.SituationBoardUI);
    }

    public void CommandSummaryBoardClose()
    {
        UIManager.Hide<SituationBoardUI>(UIList.SituationBoardUI);
    }
    #endregion
}
