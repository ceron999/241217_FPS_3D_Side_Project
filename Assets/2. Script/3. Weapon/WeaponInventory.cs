using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class WeaponInventory : MonoBehaviour
    {
        [SerializeField] private int currentWeaponSlots = 3;
        [SerializeField] private List<WeaponSlot> equipWeaponSlots = new List<WeaponSlot>();

        private void Awake()
        {
            currentWeaponSlots = 3;
        }

        public void SwitchWeapon(int inputIndex)
        {
            foreach (WeaponSlot weapon in equipWeaponSlots)
            {
                // 무기가 없으면 그냥 튕겨나감
                if (ReferenceEquals(weapon, null))
                    continue;

                weapon.AssignWeapon(inputIndex);
            }
        }
    }
}