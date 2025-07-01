using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Weapon
{
    public class InventoryCollider : MonoBehaviour
    {
        [Header("인벤토리 관련 변수")]
        private const float checkWeaponRadius = 3f;
        [SerializeField] private LayerMask weaponLayer;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(1);
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, checkWeaponRadius, weaponLayer);

            // 아무것도 탐색 안되면 제거
            if (colliders.Length == 0)
                return;

            InventoryUI.Instance.SetGroundItems(ref colliders);
        }

        private void OnTriggerExit(Collider other)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, checkWeaponRadius, weaponLayer);

            // 아무것도 탐색 안되면 제거
            if (colliders.Length == 0)
                return;

            InventoryUI.Instance.SetGroundItems(ref colliders);
        }
    }
}