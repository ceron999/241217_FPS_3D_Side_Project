using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class 요약: 플레이어/Ai가 공통으로 보유할 Status 관련 데이터
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
