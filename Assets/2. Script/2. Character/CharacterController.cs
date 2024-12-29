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

    public float throwPower = 10f;
    #endregion

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

    }

    // 칼
    void CommandAttackStart()
    {
        // 아직 애니메이션 없어서 나중에 구현
    }
    void CommandAttackEnd()
    {

    }

    // 수류탄

    /// <summary>
    /// 좌클릭을 누르면 시작
    /// 1. 던지기 모션 시작
    /// 2. 누른 위치에서 예상 투척 라인 표시
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
    /// 애니메이션 중간에 적용되는 함수
    /// 투사체를 생성하여 던짐
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

    // 무기 변환
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
