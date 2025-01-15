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
        // 1. 지형 확인
        CheckGround();

        // 3. animation 설정

        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetFloat("Horizontal", horizontal);
        characterAnimator.SetFloat("Vertical", vertical);
    }

    private void LateUpdate()
    {
        // 2. 사격
        /// 사격할 경우 총알이 불규칙하게 다른 방향으로 튀는 문제가 있었음.
        /// 해당 문제는 이 코드가 update에서 lateUpdate로 이동하니까 문제 해결
        /// 해당 문제는 Animation에서 뭔가 위치가 바뀌면서 총알도 그리 나간 것으로 보임
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
        // 앞, 뒤, 좌, 우 이동 방향 설정
        horizontal = input.x;
        vertical = input.y;
        speed = input.magnitude > 0 ? 1f : 0f;
        Vector3 movement = Vector3.zero;

        // 캐릭터가 이동할 방향벡터 설정
        movement = transform.forward * vertical + transform.right * horizontal;
        movement.y = verticalVelocity;

        unityCharacterController.Move(movement * Time.deltaTime);
    }
}