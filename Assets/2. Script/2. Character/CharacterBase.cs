using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    /// <summary>
    /// Class ���: �÷��̾�/ AI ĳ������ ���� ���� �Լ� ����
    /// </summary>


    // �̵� ���� ������
    public float moveSpeed;
    public float horizontal;
    public float vertical;

    // ȸ�� ���� ������

    // ���� ���� ������

    // ĳ���Ͱ� ������ Status
    public CharacterStatusData characterStatusData;

    // ���� ���� ������

    // �ִϸ��̼� ������

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
