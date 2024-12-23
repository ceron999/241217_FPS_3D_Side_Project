using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    /// <summary>
    /// Class 요약: 플레이어/ AI 캐릭터의 조작 관련 함수 보관
    /// </summary>


    // 이동 관련 데이터
    public float moveSpeed;
    public float horizontal;
    public float vertical;

    // 회전 관련 데이터

    // 공격 관련 데이터

    // 캐릭터가 보유한 Status
    public CharacterStatusData characterStatusData;

    // 무기 관련 데이터

    // 애니메이션 데이터

    public void Move(Vector2 input)
    {
        horizontal = input.x;
        vertical = input.y;
        Vector2 movement = Vector2.zero;
    }

    public void Rotate()
    {

    }

    public void Shoot()
    {

    }

    public void Reload()
    {

    }
}
