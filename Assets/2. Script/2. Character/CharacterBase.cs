using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    /// <summary>
    /// Class ���: �÷��̾�/ AI ĳ������ ���� ���� �Լ� ����
    /// </summary>

    // �̵� ���� ������
    public bool IsRun { get; set; } = false;
    public bool IsCrouch { get; set; } = false;

    public float speed;
    public float moveSpeed;
    public float horizontal;
    public float vertical;

    public float runningBlend;
    public float crouchBlend;

    public float rotation;

    // ȸ�� ���� ������

    // ���� ���� ������
    [SerializeField]
    protected bool isGrounded = false;
    protected float groundCheckDistance = 0.1f;
    protected float groundOffset = 0.1f;
    protected float checkRadius = 0.1f;
    [SerializeField] protected LayerMask groundLayer;

    protected float verticalVelocity;          // ������ �� Y������ �ö󰡴� �ӵ�
    protected float jumpFoice = 8;
    protected float gravity = -9.8f;

    // ���� ���� ������
    public Transform weaponHolder;
    protected WeaponBase nowWeapon;
    protected bool isShooting = false;
    protected bool isReloading = false;

    // ĳ���Ͱ� ������ Status
    public CharacterStatusData characterStatusData;

    // ���� ���� ������

    // �ִϸ��̼� ������
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
    /// �̵� ���
    /// w, a, s, d : ĳ���Ͱ� �ٶ󺸴� ������ ��, ��, ��, ��
    /// </summary>
    /// <param name="input"></param>
    public void Move(Vector2 input)
    {
        // ��, ��, ��, �� �̵� ���� ����
        horizontal = input.x;
        vertical = input.y;
        speed = input.magnitude > 0 ? SetSpeed() : 0f;
        Vector3 movement = Vector3.zero;

        // ĳ���Ͱ� �̵��� ���⺤�� ����
        movement = transform.forward * vertical + transform.right * horizontal;
        movement.y = verticalVelocity * Time.deltaTime;

        // �̵�
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
            // �ִϸ��̼� ����
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
