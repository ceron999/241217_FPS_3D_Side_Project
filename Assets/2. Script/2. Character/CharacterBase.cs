using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterBase : MonoBehaviour, IDamage
{
    /// <summary>
    /// Class 요약: 플레이어/ AI 캐릭터의 조작 관련 함수 보관
    /// </summary>
    // 플레이어인지 확인하는 변수
    public bool IsPlayer { get; private set; } = false;
    public int characterIndex;

    // 이동 관련 데이터
    public bool IsDie { get; set; } = false;
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

    [Header("무기")]
    public Transform weaponHolder;
    public WeaponBase nowWeapon;
    protected bool isShooting = false;
    protected bool isReloading = false;

    [Header("스테이터스")]
    public CharacterStatusData maxStat;
    public CharacterStatusData curStat;

    public Vector3 AimingPoint 
    { 
        get => aimingPointTransform.position; 
        set => aimingPointTransform.position = value;
    }
    [Header("에임 데이터")]
    public Transform aimingPointTransform;

    [Header("애니메이션 데이터")]
    public Animator characterAnimator;
    public UnityEngine.CharacterController unityCharacterController;
    public RigBuilder rigBuilder;
    public Rig aimRig;

    [Header("음향")]
    public AudioSource audioSource;

    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
        unityCharacterController = GetComponent<UnityEngine.CharacterController>();
        rigBuilder = GetComponent<RigBuilder>();
        audioSource = GetComponent<AudioSource>();

        curStat = ScriptableObject.CreateInstance<CharacterStatusData>();
        curStat.HP = maxStat.HP;
        curStat.WalkSpeed = maxStat.WalkSpeed;
        curStat.RunSpeed = maxStat.RunSpeed;
        curStat.CrouchSpeed = maxStat.CrouchSpeed;
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

    //public void Jump()
    //{
    //    if(isGrounded)
    //    {
    //        verticalVelocity = jumpFoice;
    //        characterAnimator.SetTrigger("Jump Trigger");
    //        // 애니메이션 구동
    //    }
    //}

    public void Shoot(bool isShoot)
    {
        if(!IsRun)
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

    public virtual void ApplyDamage(float getDamage)
    {
        if (IsDie)
            return;

        //데미지 받음
        curStat.HP -= getDamage;
        Debug.Log($"curr hp : {curStat.HP}");
        if (curStat.HP <= 0)
        {
            // 사망
            aimRig.weight = 0;
            characterAnimator.SetTrigger("Dead Trigger");
            IsDie = true;
        }
    }
}
