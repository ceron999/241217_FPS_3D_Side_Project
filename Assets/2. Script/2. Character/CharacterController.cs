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
        InputSystem.Instance.OnClickLeftMouseButtonDown += CommandZoomIn;       // 줌인

        // 스위칭
        InputSystem.Instance.OnClickAlpha1 += CommandSwitchMainWeapon;          // 주 무기 변환
        InputSystem.Instance.OnClickAlpha2 += CommandSwitchPistol;              // 권총 변환
        InputSystem.Instance.OnClickAlpha3 += CommandSwitchKnife;               // 칼 변환
        InputSystem.Instance.OnClickAlpha4 += CommandSwitchGrenade;             // 수류탄 변환
        InputSystem.Instance.OnClickAlpha5 += CommandSwitchC4;                  // C4 변환

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

    // 무기 변환
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
