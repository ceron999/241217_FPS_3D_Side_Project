using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Weapon;

namespace UI
{
    public class InventoryUI : UIBase
    {
        public  static InventoryUI Instance => UIManager.Singleton.GetUI<InventoryUI>(UIList.InventoryUI);

        [Header("Pool Size")]
        [SerializeField] private const int defaultSize = 5;
        [SerializeField] private const int maxPoolSize = 15;

        [Header("인벤토리")]
        [SerializeField] private WeaponItemPrefab _weaponItemPrefab;
        [SerializeField] private Transform groundItemParent;
        private IObjectPool<WeaponItemPrefab> groundItemPool;
        [SerializeField] private Transform InventoryItemParent;
        private IObjectPool<WeaponItemPrefab> inventoryItemPool;

        [Header("착용 파츠")]
        [SerializeField] private Image headPartsImage;
        [SerializeField] private Image armorPartsImage;

        [Header("착용 무기")]
        [SerializeField] private GameObject[] weaponButtons;

        private void Awake()
        {
            InitPool();
        }

        public override void Show()
        {
            base.Show();
        }
        public override void Hide()
        {
            base.Hide();
        }

        public void SetGroundItems(ref Collider[] groundItems)
        {
            for (int i = 0; i < groundItemParent.childCount; i++)
            {
                groundItemParent.GetChild(i).GetComponent<WeaponItemPrefab>().returnToPoolCallBack?.Invoke();
            }

            for (int i = 0; i < groundItems.Length; i++)
            {
                ShowGroundItem(groundItems[i].GetComponent<WeaponSlot>());
            }
        }

        #region Pool Func
        private void InitPool()
        {
            groundItemPool = new ObjectPool<WeaponItemPrefab>
                (
                    createFunc: CreatePooledGroundItem,
                    actionOnGet: OnTakeFromPool,
                    actionOnRelease: OnReturnedToPool,
                    actionOnDestroy: OnDestroyPoolObject,
                    collectionCheck: true,
                    defaultCapacity: defaultSize,
                    maxSize: maxPoolSize
                );

            inventoryItemPool = new ObjectPool<WeaponItemPrefab>
                (
                    createFunc: CreatePooledInventoryItem,
                    actionOnGet: OnTakeFromPool,
                    actionOnRelease: OnReturnedToPool,
                    actionOnDestroy: OnDestroyPoolObject,
                    collectionCheck: true,
                    defaultCapacity: defaultSize,
                    maxSize: maxPoolSize
                );
        }


        WeaponItemPrefab CreatePooledGroundItem()
        {
            WeaponItemPrefab weaponItem = Instantiate(_weaponItemPrefab);
            weaponItem.transform.SetParent(groundItemParent);
            weaponItem.transform.localScale = Vector3.one;

            return weaponItem;
        }

        WeaponItemPrefab CreatePooledInventoryItem()
        {
            WeaponItemPrefab weaponItem = Instantiate(_weaponItemPrefab);
            weaponItem.transform.SetParent(InventoryItemParent);

            return weaponItem;
        }

        void OnReturnedToPool(WeaponItemPrefab weaponItem)
        {
            weaponItem.gameObject.SetActive(false);
        }

        void OnTakeFromPool(WeaponItemPrefab weaponItem)
        {
            weaponItem.gameObject.SetActive(true);
        }

        void OnDestroyPoolObject(WeaponItemPrefab weaponItem)
        {
            Destroy(weaponItem.gameObject);
        }

        public void ShowGroundItem(WeaponSlot weapon)
        {
            WeaponItemPrefab item = groundItemPool.Get();
            item.UpdateWeaponItemPrefab(weapon);
            item.Init(() => groundItemPool.Release(item));  // bullet 내부에서 사용 후 반납
        }

        public void ShowInventoryItem()
        {
            WeaponItemPrefab item = inventoryItemPool.Get();
            item.Init(() => inventoryItemPool.Release(item));  // bullet 내부에서 사용 후 반납
        }
        #endregion
    }
}