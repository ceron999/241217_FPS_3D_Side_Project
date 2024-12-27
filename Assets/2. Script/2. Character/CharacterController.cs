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

    // ���� ��ȯ
    public void CommandSwitchMainWeapon()
    {
        player.SwitchMainWeapon();
    }

    public void CommandSwitchPistol()
    {
        player.SwitchPistol();
    }

    public void CommandSwitchKnife()
    {
        player.SwitchKnife();
    }

    public void CommandSwitchGrenade()
    {
        player.SwitchGrenade();
    }

    public void CommandSwitchC4()
    {
        player.SwitchC4();
    }

    #endregion
}
