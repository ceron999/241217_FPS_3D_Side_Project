using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] int slotIndex;
        public GameObject slotWeapon;

        // 현재 무기를 사용하도록 설정합니다. 
        public virtual void AssignWeapon(int weaponIndex)
        {
            if (weaponIndex != slotIndex)
            {
                slotWeapon.SetActive(false);
            }
            else
            {
                slotWeapon.SetActive(true);
            }
        }

        public virtual void ThrowAwayWeapon(int weaponIndex)
        {
            if (weaponIndex == slotIndex)
            {
                slotWeapon = null;
            }
        }
    }
}