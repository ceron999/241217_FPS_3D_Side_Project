using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class WeaponInventory : MonoBehaviour
    {
        [SerializeField] private int currentWeaponSlot = 3;
        [SerializeField] private List<WeaponSlot> equipWeaponSlots = new List<WeaponSlot>();
        public WeaponBase CurrWeapon 
        { 
            get 
            {
                if (ReferenceEquals(currWeapon, null))
                {
                    // 처음 가진
                    currWeapon = equipWeaponSlots[currentWeaponSlot].slotWeapon;
                }
                    return currWeapon;
            } 
            private set
            {
                
                    currWeapon = value;
            }
        }
        [SerializeField] private WeaponBase currWeapon;

        public WeaponSlot SwitchWeapon(int inputIndex)
        {
            currentWeaponSlot = inputIndex;

            foreach (WeaponSlot weapon in equipWeaponSlots)
            {
                // 무기가 없으면 그냥 튕겨나감
                if (ReferenceEquals(weapon, null))
                    continue;

                weapon.AssignWeapon(inputIndex);
            }

            CurrWeapon = equipWeaponSlots[currentWeaponSlot].slotWeapon;

            return equipWeaponSlots[currentWeaponSlot];
        }
    }
}