using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapon;

namespace UI
{
    public class WeaponItemPrefab : MonoBehaviour
    {
        // Pool Action
        public bool IsReleased { get; private set; }
        public System.Action returnToPoolCallBack;

        [Header("아이템 UI 정보")]
        [SerializeField] private Image weaponImage;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI weaponAmmoText;

        public void Init(System.Action returnToPool)
        {
            this.returnToPoolCallBack = () =>
            {
                if (!IsReleased)
                {
                    returnToPool?.Invoke();
                    IsReleased = true;
                }
            };
            IsReleased = false;
        }

        public void ShowWeaponItem(WeaponSlot weapon)
        {
            weaponImage.sprite = weapon.weaponSprite;
            weaponNameText.text = weapon.slotWeapon.name;
        }

        public void Release() => returnToPoolCallBack?.Invoke();
    }
}