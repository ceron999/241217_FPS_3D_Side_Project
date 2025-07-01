using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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

        // 주변에 위치한 아이템 UI
        [SerializeField] private Transform groundItemParent;
        private IObjectPool<WeaponItemPrefab> groundItemPool;

        // 보유한 아이템 UI
        [SerializeField] private Transform InventoryItemParent;
        private IObjectPool<WeaponItemPrefab> inventoryItemPool;

        // <Weapon의 instance ID, 주변 아이템의 UI 프리팹>로 이루어진 Dict
        private Dictionary<int, WeaponItemPrefab> itemDictionary = new Dictionary<int, WeaponItemPrefab>();

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

            // 커서 표시
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public override void Hide()
        {
            // 커서 제거
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            foreach (var item in itemDictionary.Values)
            {
                item.Release();
            }

            itemDictionary.Clear();
            base.Hide();
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
            int weaponID = weapon.GetInstanceID();
            if(itemDictionary.ContainsKey(weaponID))
            {
                itemDictionary[weaponID].ShowWeaponItem(weapon);
                return;
            }

            WeaponItemPrefab item = groundItemPool.Get();
            item.ShowWeaponItem(weapon);
            item.Init(() => groundItemPool.Release(item));  // bullet 내부에서 사용 후 반납

            itemDictionary.Add(weapon.GetInstanceID(), item);
        }

        public void HideGroundItem(WeaponSlot weapon)
        {
            int weaponID = weapon.GetInstanceID();
            if(itemDictionary.ContainsKey(weaponID))
            {
                itemDictionary[weaponID].Release();
                itemDictionary.Remove(weaponID);
            }
        }

        public void ShowInventoryItem()
        {
            WeaponItemPrefab item = inventoryItemPool.Get();
            item.Init(() => inventoryItemPool.Release(item));  // bullet 내부에서 사용 후 반납
        }
        #endregion
    }
}