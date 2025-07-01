using Character.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Animations.Rigging;
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
    [SerializeField] private Vector3 throwDirection;
    [SerializeField] private Vector3 initialVelocity;
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
        InputSystem.Instance.OnOpenInventory += CommandOpenInventory;
        InputSystem.Instance.OnCloseInventory += CommandCloseInventory;

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

        if (isThrowMode)
        {
            throwDirection = (player.aimingPointTransform.position - throwStartPivot.position).normalized;
            throwDirection.y += 1.0f; // 약간 위쪽으로 던지는 각도 추가
            ShowTrajectory(throwStartPivot.position, throwDirection * throwPower);
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
            grenadeLineRenderer.gameObject.SetActive(true);

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
        Grenade throwObject = Instantiate(throwObjectPrefab).GetComponent<Grenade>();

        throwObject.transform.position = throwStartPivot.position;
        throwObject.throwStartPivot = throwStartPivot;
        throwObject.throwPower = throwPower;
        throwObject.throwVector = throwDirection;

        throwObject.Activate();
    }

    /// <summary>
    /// 수류탄 궤적 표시
    /// </summary>
    /// <param name="startPosition">투척 시작 위치</param>
    /// <param name="initialVelocity">초기 속도 (방향 * 힘)</param>
    private void ShowTrajectory(Vector3 startPosition, Vector3 initialVelocity)
    {
        Vector3 currentPosition = startPosition;
        Vector3 velocity = initialVelocity; // 초기 속도
        float timeStep = 0.1f;              // 궤적 샘플링 간격

        for (int i = 0; i < linePoint; i++)
        {
            // 궤적의 현재 위치 계산
            grenadeLineRenderer.SetPosition(i, currentPosition);

            // 위치 업데이트: 포물선 운동 (s = ut + 0.5 * a * t^2, v = u + at)
            currentPosition += velocity * timeStep;
            velocity += Physics.gravity * timeStep; // 중력 적용
        }

        // LineRenderer 활성화
        grenadeLineRenderer.enabled = true;
    }

    void CommandThrowEnd()
    {
        if (isThrowMode)
        {
            player.ThrowEnd();
            isThrowMode = false;
            isThrowEnd = true;

            grenadeLineRenderer.gameObject.SetActive(false);
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


    // 인벤토리 표시 및 끄기
    public void CommandOpenInventory()
    {
        UIManager.Show<InventoryUI>(UIList.InventoryUI);
    }
    public void CommandCloseInventory()
    {
        UIManager.Hide<InventoryUI>(UIList.InventoryUI);
    }
    #endregion
}
