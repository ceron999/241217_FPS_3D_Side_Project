using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : CharacterBase
{

    private void Start()
    {
        nowWeapon = weaponHolder.GetChild(0).GetComponent<WeaponBase>();
    }
    private void Update()
    {
        // 1. ���� Ȯ��
        CheckGround();

        // 3. animation ����

        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetFloat("Horizontal", horizontal);
        characterAnimator.SetFloat("Vertical", vertical);
    }

    public void AIMove(Vector2 input)
    {
        // ��, ��, ��, �� �̵� ���� ����
        horizontal = input.x;
        vertical = input.y;
        speed = input.magnitude > 0 ? 1f : 0f;
        Vector3 movement = Vector3.zero;

        // ĳ���Ͱ� �̵��� ���⺤�� ����
        movement = transform.forward * vertical + transform.right * horizontal;
        movement.y = verticalVelocity * Time.deltaTime;

        // �̵�
        unityCharacterController.Move(movement * moveSpeed * Time.deltaTime);
    }
}