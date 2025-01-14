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
        // 1. 지형 확인
        CheckGround();

        // 3. animation 설정

        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetFloat("Horizontal", horizontal);
        characterAnimator.SetFloat("Vertical", vertical);
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
        movement.y = verticalVelocity * Time.deltaTime;

        // 이동
        unityCharacterController.Move(movement * moveSpeed * Time.deltaTime);
    }
}