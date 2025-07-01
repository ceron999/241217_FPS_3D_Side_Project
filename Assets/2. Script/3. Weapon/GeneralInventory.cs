using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class GeneralInventory : MonoBehaviour
    {
        [Header("인벤토리 관련 변수")]
        private const float checkWeaponRadius = 3f;
        [SerializeField] private LayerMask weaponLayer;

        // Update is called once per frame
        void Update()
        {
            CheckSurroundingItems();
        }

        private void CheckSurroundingItems()
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, checkWeaponRadius, weaponLayer);

            // 아무것도 탐색 안되면 제거
            if (colliders.Length == 0)
                return;

            // 범위 안에서 에임을 둔 무기가 1순위
            if (IsCheckAimedItem())
                return;

            // 이후 가까운 순서대로 2순위
            int minColliderIndex = 0;
            float minDistance = int.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                float sqrDist = (colliders[i].transform.position - transform.position).sqrMagnitude;
                if (sqrDist < minDistance)
                {
                    minColliderIndex = i;
                    minDistance = sqrDist;
                }
            }

            if (colliders[minColliderIndex].TryGetComponent<WeaponSlot>(out WeaponSlot minDistWeapon))
            {
                minDistWeapon.CheckWeapon();
            }
        }

        private bool IsCheckAimedItem()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, weaponLayer, QueryTriggerInteraction.Ignore))
            {
                // 범위 안에 들어오고 weaponslot을 가지고 있으면 반환
                if ((hitInfo.transform.position - transform.position).sqrMagnitude < checkWeaponRadius * checkWeaponRadius)
                {
                    if (hitInfo.collider.TryGetComponent<WeaponSlot>(out WeaponSlot raycastWeapon))
                    {
                        raycastWeapon.CheckWeapon();
                        return true;
                    }
                }

            }

            return false;
        }
    }
}