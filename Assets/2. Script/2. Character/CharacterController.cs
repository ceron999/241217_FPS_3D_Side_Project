using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class ���: player�� ���� ���� �Լ� ü�� �� ��üȭ
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
