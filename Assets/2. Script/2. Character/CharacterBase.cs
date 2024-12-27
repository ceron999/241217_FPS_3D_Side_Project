using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    /// <summary>
    /// Class 요약: 플레이어/ AI 캐릭터의 조작 관련 함수 보관
    /// </summary>

    // 이동 관련 데이터
    public bool IsRun { get; set; } = false;
    public bool IsCrouch { get; set; } = false;

    public float speed;
    public float moveSpeed;
    public float horizontal;
    public float vertical;

    public float runningBlend;
    public float crouchBlend;

    public float rotation;

    // 회전 관련 데이터

    // 점프 관련 데이터
    [SerializeField]
    protected bool isGrounded = false;
    protected float groundCheckDistance = 0.1f;
    protected float groundOffset = 0.1f;
    protected float checkRadius = 0.1f;
    [SerializeField] protected LayerMask groundLayer;

    protected float verticalVelocity;          // 점프할 때 Y축으로 올라가는 속도
    protected float jumpFoice = 8;
    protected float gravity = -9.8f;

    // 공격 관련 데이터
    public Transform weaponHolder;
    protected WeaponBase nowWeapon;
    protected bool isShooting = false;
    protected bool isReloading = false;

    // 캐릭터가 보유한 Status
    public CharacterStatusData characterStatusData;

    // 무기 관련 데이터

    // 애니메이션 데이터
    public Animator characterAnimator;
    public UnityEngine.CharacterController unityCharacterController;


    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
        unityCharacterController = GetComponent<UnityEngine.CharacterController>();
    }

    private void Start()
    {
        nowWeapon = weaponHolder.GetChild(0).GetComponent<WeaponBase>();
    }

    /// <summary>
    /// 이동 방식
    /// w, a, s, d : 캐릭터가 바라보는 방향의 앞, 뒤, 좌, 우
    /// </summary>
    /// <param name="input"></param>
    public void Move(Vector2 input)
    {
        // 앞, 뒤, 좌, 우 이동 방향 설정
        horizontal = input.x;
        vertical = input.y;
        speed = input.magnitude > 0 ? SetSpeed() : 0f;
        Vector3 movement = Vector3.zero;

        // 캐릭터가 이동할 방향벡터 설정
        movement = transform.forward * vertical + transform.right * horizontal;
        movement.y = verticalVelocity * Time.deltaTime;

        // 이동
        unityCharacterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    public float SetSpeed()
    {
        if (IsRun)
            return 8;
        else if (IsCrouch)
            return 2;
        else
            return 4;
    }

    public void SetRunning(bool isRunning)
    {
        IsRun = isRunning;
    }
    public void SetCrouch(bool isCrouching)
    {
        IsCrouch = isCrouching;
    }

    public void Rotate(float angle)
    {
        rotation += angle;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            verticalVelocity = jumpFoice;
            characterAnimator.SetTrigger("Jump Trigger");
            // 애니메이션 구동
        }
    }

    public void Shoot(bool isShoot)
    {
        isShooting = isShoot;
    }

    public void Reload()
    {
        isReloading = true;
        characterAnimator.SetTrigger("Reload Trigger");
    }

    public void ReloadComplete()
    {
        nowWeapon.Reload();
        isReloading = false;
    }

    protected void CheckGround()
    {
        Ray ray = new Ray(transform.position + (Vector3.up * groundOffset), Vector3.down);
        isGrounded = Physics.SphereCast(ray, checkRadius, 0.1f, groundLayer);

        if(isGrounded)
        {
            verticalVelocity = 0;
        }
        else
        {
            verticalVelocity = gravity;
        }
    }
}
