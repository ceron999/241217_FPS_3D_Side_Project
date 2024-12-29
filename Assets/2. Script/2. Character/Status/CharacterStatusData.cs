using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class ���: �÷��̾�/Ai�� �������� ������ Status ���� ������
/// </summary>

[System.Serializable]
[CreateAssetMenu(fileName = "CharacterStatusData", menuName = "Character/Status")]
public class CharacterStatusData : ScriptableObject
{
    public float HP;

    public float WalkSpeed;
    public float RunSpeed;
    public float CrouchSpeed;
}
