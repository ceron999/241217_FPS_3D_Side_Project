using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// class ���: player�� ���� ���� �Լ� ü�� �� ��üȭ
/// </summary>

public class CharacterController : MonoBehaviour
{
    public PlayerBase player;

    #region ���� ����
    public Transform throwStartPivot;
    public GameObject throwObjectPrefab;
    private bool isThrowMode = false;
    private bool isThrowEnd = false;

    public float throwPower = 10f;
    #endregion

    #region ī�޶� ����
    public Transform cameraPivot;
    public float bottomClamp = -90f;
    public float topClamp = 90f;
    private float targetYaw;
    private float targetpitch;
    #endregion

    #region C4 ��ġ ����
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
        InputSystem.Instance.OnClickSpace += CommandJump;                       // ����
        InputSystem.Instance.OnClickR += CommandReload;                         // ������

        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandFireStart;    // ���
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandFireStop;       // ��� ����

        if(GameManager.StartData.startMainWeaponType == MainWeaponType.Sniper)
            InputSystem.Instance.OnClickRightMouseButtonDown += CommandZoomIn;       // ���� (���������� ��츸)

        // ����Ī
        InputSystem.Instance.OnClickAlpha1 += CommandSwitchMainWeapon;          // �� ���� ��ȯ
        InputSystem.Instance.OnClickAlpha2 += CommandSwitchPistol;              // ���� ��ȯ
        InputSystem.Instance.OnClickAlpha3 += CommandSwitchGrenade;             // ����ź ��ȯ
        InputSystem.Instance.OnClickAlpha4 += CommandSwitchC4;                  // C4 ��ȯ

        InputSystem.Instance.OnClickTabDown += CommandSummaryBoardOpen;         // ��Ȳ�� Ű��
        InputSystem.Instance.OnClickTabUp += CommandSummaryBoardClose;          // ��Ȳ�� ����

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

    // ��
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

    // ����ź

    /// <summary>
    /// ��Ŭ���� ������ ����
    /// 1. ������ ��� ����
    /// 2. ���� ��ġ���� ���� ��ô ���� ǥ��
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
    /// �ִϸ��̼� �߰��� ����Ǵ� �Լ�
    /// ����ü�� �����Ͽ� ����
    /// </summary>
    public void ThrowPrefab()
    {
        GameObject throwObject = Instantiate(throwObjectPrefab, throwStartPivot);
        if(throwObject.TryGetComponent<Grenade>(out Grenade grenadeComponent))
        {
            // ��ô ���� ����, ��ô �� ���� 
            grenadeComponent.throwStartPivot = throwStartPivot;
            grenadeComponent.throwPower = throwPower;

            // ��ô ���� ����
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
        // ��ġ ��ҿ��� ���� �ʴٸ� ����
        if (Vector3.Distance(installPosition.position, this.transform.position) < installRadius)
        {
            BattleManager.Instance.c4InstallPosition = installPosition;
            player.nowWeapon.Activate();
        }
    }

    // ���� ��ȯ
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
        // ����ź�� ����ߴٸ� ����Ī ���ϵ���
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


    // ��Ȳ�� ǥ�� �� ����
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
