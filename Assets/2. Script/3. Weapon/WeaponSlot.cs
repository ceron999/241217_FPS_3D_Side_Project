using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Weapon
{
    public class WeaponSlot : MonoBehaviour
    {
        public bool isPickedUp = false;

        [Header("인벤토리 데이터")]
        [SerializeField] private LayerMask inventoryLayer;

        [Header("슬롯 데이터")]
        public Sprite weaponSprite;
        [SerializeField] int slotIndex;
        public WeaponBase slotWeapon;

        [Header("애니메이션 데이터")]
        [SerializeField] private AnimVariables animVariables;

        private void Awake()
        {
            slotWeapon = GetComponent<WeaponBase>();
        }

        #region 인벤토리 충돌 감지
        private void OnTriggerEnter(Collider other)
        {
                Debug.Log(other.name);
            if (((1 << other.gameObject.layer) & inventoryLayer.value) != 0)
            {
                Debug.Log(this.name);
                InventoryUI.Instance.ShowGroundItem(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & inventoryLayer.value) != 0)
            {
                Debug.Log(this.name);
                InventoryUI.Instance.HideGroundItem(this);
            }
        }
        #endregion

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

        public virtual AnimVariables GetAnimVariables()
        {
            return animVariables;
        }

        public void CheckWeapon()
        {
            if (isPickedUp)
                return;

            Debug.Log(slotWeapon.name);
        }
    }
}