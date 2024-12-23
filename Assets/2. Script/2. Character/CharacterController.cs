using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        player.Move(InputSystem.Instance.Movement);
    }

    private void Update()
    {
        
    }
}
