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

    public float throwPower = 10f;
    #endregion

    #region ī�޶� ����
    public Transform cameraPivot;
    public float bottomClamp = -90f;
    public float topClamp = 90f;
    private float targetYaw;
    private float targetpitch;
    #endregion

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
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandZoomIn;       // ����

        // ����Ī
        InputSystem.Instance.OnClickAlpha1 += CommandSwitchMainWeapon;          // �� ���� ��ȯ
        InputSystem.Instance.OnClickAlpha2 += CommandSwitchPistol;              // ���� ��ȯ
        InputSystem.Instance.OnClickAlpha3 += CommandSwitchKnife;               // Į ��ȯ
        InputSystem.Instance.OnClickAlpha4 += CommandSwitchGrenade;             // ����ź ��ȯ
        InputSystem.Instance.OnClickAlpha5 += CommandSwitchC4;                  // C4 ��ȯ

        InputSystem.Instance.OnClickTabDown += CommandSummaryBoardOpen;         // C4 ��ȯ
        InputSystem.Instance.OnClickTabUp += CommandSummaryBoardClose;          // C4 ��ȯ

        CameraSystem.Singleton.SetCameraFollowTarget(cameraPivot);
    }

    private void Update()
    {
        player.SetRunning(InputSystem.Instance.IsRun);
        player.SetCrouch(InputSystem.Instance.IsCrouch);
        player.Move(InputSystem.Instance.Movement);

        player.Rotate(InputSystem.Instance.Look.x);
        player.AimingPoint = CameraSystem.Singleton.AimingPoint;
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
        player.Jump();
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

    }

    // Į
    void CommandAttackStart()
    {
        // ���� �ִϸ��̼� ��� ���߿� ����
    }
    void CommandAttackEnd()
    {

    }

    // ����ź

    /// <summary>
    /// ��Ŭ���� ������ ����
    /// 1. ������ ��� ����
    /// 2. ���� ��ġ���� ���� ��ô ���� ǥ��
    /// </summary>
    void CommandThrowStart()
    {
        if (!isThrowMode)
        {
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
            grenadeComponent.throwStartPivot = throwStartPivot;
            grenadeComponent.throwPower = throwPower;
            grenadeComponent.Activate();
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
        }
    }

    // ���� ��ȯ
    public void CommandSwitchMainWeapon()
    {
        UIManager.Show<WeaponUI>(UIList.WeaponUI);
        BulletUI.Instance.ChangeWeapon(player.nowWeapon);

        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandFireStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandFireStop;
        player.SwitchMainWeapon();

        
    }

    public void CommandSwitchPistol()
    {
        UIManager.Show<WeaponUI>(UIList.WeaponUI);
        BulletUI.Instance.ChangeWeapon(player.nowWeapon);

        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandFireStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandFireStop;
        player.SwitchPistol();
    }

    public void CommandSwitchKnife()
    {
        UIManager.Show<WeaponUI>(UIList.WeaponUI);
        BulletUI.Instance.ChangeWeapon(player.nowWeapon);

        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandAttackStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandAttackEnd;
        player.SwitchKnife();
    }

    public void CommandSwitchGrenade()
    {
        UIManager.Show<WeaponUI>(UIList.WeaponUI);
        BulletUI.Instance.ChangeWeapon(player.nowWeapon);

        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandThrowStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandThrowEnd;
        player.SwitchGrenade();
    }

    public void CommandSwitchC4()
    {
        UIManager.Show<WeaponUI>(UIList.WeaponUI);
        BulletUI.Instance.ChangeWeapon(player.nowWeapon);

        player.SwitchC4();
    }


    // ��Ȳ�� ǥ�� �� ����
    public void CommandSummaryBoardOpen()
    {
        UIManager.Show<SummaryBoardUI>(UIList.SummaryBoardUI);
    }

    public void CommandSummaryBoardClose()
    {
        UIManager.Hide<SummaryBoardUI>(UIList.SummaryBoardUI);
    }
    #endregion
}
