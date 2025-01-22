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
    public Transform throwStartPivot;
    public GameObject throwObjectPrefab;
    private bool isThrowMode = false;
    private bool isThrowEnd = false;

    public float throwPower = 10f;
    #endregion

    #region 카메라 변수
    public Transform cameraPivot;
    public float bottomClamp = -90f;
    public float topClamp = 90f;
    private float targetYaw;
    private float targetpitch;
    #endregion

    #region C4 설치 변수
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
        InputSystem.Instance.OnClickSpace += CommandJump;                       // 점프
        InputSystem.Instance.OnClickR += CommandReload;                         // 재장전

        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandFireStart;    // 사격
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandFireStop;       // 사격 중지

        if(GameManager.StartData.startMainWeaponType == MainWeaponType.Sniper)
            InputSystem.Instance.OnClickRightMouseButtonDown += CommandZoomIn;       // 줌인 (스나이퍼일 경우만)

        // 스위칭
        InputSystem.Instance.OnClickAlpha1 += CommandSwitchMainWeapon;          // 주 무기 변환
        InputSystem.Instance.OnClickAlpha2 += CommandSwitchPistol;              // 권총 변환
        InputSystem.Instance.OnClickAlpha3 += CommandSwitchGrenade;             // 수류탄 변환
        InputSystem.Instance.OnClickAlpha4 += CommandSwitchC4;                  // C4 변환

        InputSystem.Instance.OnClickTabDown += CommandSummaryBoardOpen;         // 상황판 키기
        InputSystem.Instance.OnClickTabUp += CommandSummaryBoardClose;          // 상황판 끄기

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
            CameraSystem.Instance.tpsCamera.m_Lens.FieldOfView = 20f;
            ZoomUI.Instance.Show();
        }
        else
        {
            CameraSystem.Instance.tpsCamera.m_Lens.FieldOfView = 40f;
            ZoomUI.Instance.Hide();
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

            WeaponUI.Instance.SetGrenadeUIOff();
            InputSystem.Instance.OnClickAlpha3 = null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
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
        // 설치 장소에서 멀지 않다면 실행
        if (Vector3.Distance(installPosition.position, this.transform.position) < installRadius)
        {
            BattleManager.Instance.c4InstallPosition = installPosition;
            player.nowWeapon.Activate();
        }
    }

    // 무기 변환
    public void CommandSwitchMainWeapon()
    {
        UIManager.Show<WeaponUI>(UIList.WeaponUI);

        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButton = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandFireStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandFireStop;
        player.SwitchMainWeapon();

        BulletUI.Instance.ChangeWeapon(player.nowWeapon);
    }

    public void CommandSwitchPistol()
    {
        UIManager.Show<WeaponUI>(UIList.WeaponUI);

        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButton = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandFireStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandFireStop;
        player.SwitchPistol();

        BulletUI.Instance.ChangeWeapon(player.nowWeapon);
    }

    public void CommandSwitchGrenade()
    {
        // 수류탄을 사용했다면 스위칭 못하도록
        if (isThrowEnd)
            return;

        UIManager.Show<WeaponUI>(UIList.WeaponUI);

        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButton = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandThrowStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandThrowEnd;
        player.SwitchGrenade();

        BulletUI.Instance.ChangeWeapon(player.nowWeapon);
    }

    public void CommandSwitchC4()
    {
        UIManager.Show<WeaponUI>(UIList.WeaponUI);

        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButton = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandInstallC4;
        player.SwitchC4();

        BulletUI.Instance.ChangeWeapon(player.nowWeapon);
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
