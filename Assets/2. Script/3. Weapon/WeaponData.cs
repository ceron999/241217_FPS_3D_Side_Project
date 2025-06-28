using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData", order = 0)]
    [Tooltip("무기 스펙 정의용 SO")]
    public class WeaponData : ScriptableObject
    {
        public string WeaponName
        {
            get
            {
                if (!weaponName.IsNullOrWhitespace())
                    return weaponName;
                else
                {
                    Debug.LogError($"Weapon Name이 적절하지 않습니다");
                    return null;
                }
            }
        }
        public string weaponName;

        public GameObject weaponPrefab;

        [Header("무기 스펙")]
        public float weaponDamage;      // 무기 데미지

        [Header("탄창 보유량")]
        public int maxAmmo;             // 무기의 최대 보유량
        public int holdAmmo;            // 앞으로 사용 가능한 보유량
        public int remainAmmo;          // 현재 사용 중인 무기의 남은 개수(현재 탄창의 남은 탄 개수
        public int clipSize;            // 탄창 크기
    }
}