using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : CharacterBase
{
    private float aiSpeed = 2f;

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

    private void LateUpdate()
    {
        // 2. ���
        /// ����� ��� �Ѿ��� �ұ�Ģ�ϰ� �ٸ� �������� Ƣ�� ������ �־���.
        /// �ش� ������ �� �ڵ尡 update���� lateUpdate�� �̵��ϴϱ� ���� �ذ�
        /// �ش� ������ Animation���� ���� ��ġ�� �ٲ�鼭 �Ѿ˵� �׸� ���� ������ ����
        if (isShooting)
        {
            bool isFireSuccess = nowWeapon.Activate();
            if (false == isFireSuccess)
            {
                if (nowWeapon.holdAmmo <= 0)
                    return;

                if (nowWeapon.RemainAmmo <= 0 && false == isReloading)
                {
                    isReloading = true;
                    Reload();
                }
            }
        }

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
        movement.y = verticalVelocity;

        unityCharacterController.Move(movement * Time.deltaTime);
    }
}