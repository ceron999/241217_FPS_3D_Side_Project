using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class WeaponSlot : MonoBehaviour
    {
        [Header("슬롯 데이터")]
        [SerializeField] int slotIndex;
        public WeaponBase slotWeapon;

        [Header("애니메이션 데이터")]
        [SerializeField] private int rifleBlend;
        [SerializeField] private float rigWeight;

        private void Awake()
        {
            slotWeapon = GetComponent<WeaponBase>();
        }

        // 현재 무기를 사용하도록 설정합니다. 
        public virtual void AssignWeapon(int weaponIndex)
        {
            if (weaponIndex != slotIndex)
            {
                slotWeapon.gameObject.SetActive(false);
            }
            else
            {
                slotWeapon.gameObject.SetActive(true);
            }
        }

        public virtual void ThrowAwayWeapon(int weaponIndex)
        {
            if (weaponIndex == slotIndex)
            {
                // TODO: 버리고싶은 무기를 바닥에 버린 후 현재 슬롯을 비운다
                slotWeapon = null;
            }
        }

        public virtual (int, float) GetAnimVariables()
        {
            return (rifleBlend, rigWeight);
        }
    }
}