using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    [System.Serializable]
    public struct AnimVariables 
    {
        [Header("애니메이션 데이터")]
        public int rifleBlend;
        public float rigWeight;

        [Header("IK Info")]
        public Vector3 weapon_LeftHandIk_Target_Position;
        public Vector3 weapon_LeftHandIk_Target_Rotation;
    }
}