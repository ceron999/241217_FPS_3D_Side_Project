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

    }

    private void Update()
    {
        player.SetRunning(InputSystem.Instance.IsRun);
        player.SetCrouch(InputSystem.Instance.IsCrouch);
        player.Move(InputSystem.Instance.Movement);

        player.Rotate(InputSystem.Instance.Look.x);
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
        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandFireStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandFireStop;
        player.SwitchMainWeapon();
    }

    public void CommandSwitchPistol()
    {
        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandFireStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandFireStop;
        player.SwitchPistol();
    }

    public void CommandSwitchKnife()
    {
        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandAttackStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandAttackEnd;
        player.SwitchKnife();
    }

    public void CommandSwitchGrenade()
    {
        InputSystem.Instance.OnClickLeftMouseButtonDown = null;
        InputSystem.Instance.OnClickLeftMouseButtonUp = null;
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandThrowStart;
        InputSystem.Instance.OnClickLeftMouseButtonUp += CommandThrowEnd;
        player.SwitchGrenade();
    }

    public void CommandSwitchC4()
    {
        player.SwitchC4();
    }

    #endregion
}
