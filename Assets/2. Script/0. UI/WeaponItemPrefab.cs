using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Weapon;

namespace UI
{
    public class WeaponItemPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // Pool Action
        public bool IsReleased { get; private set; }
        public System.Action returnToPoolCallBack;

        [Header("아이템 UI 정보")]
        [SerializeField] private Image weaponImage;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI weaponAmmoText;


        [Header("아이템 UI 드래그 변수")]
        private RectTransform rectTransform;
        [SerializeField] private RectTransform inventoryRectTransform;
        [SerializeField] private Transform beforeParent;
        [SerializeField] private Vector3 originalUIBeforePosition;
        [SerializeField] private Vector2 originalLocalPointerPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            inventoryRectTransform = InventoryUI.Instance.GetComponent<RectTransform>();
        }

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



        #region Mouse Actions
        public void OnBeginDrag(PointerEventData eventData)
        {
            beforeParent = transform.parent;

            // UI 튀는거 방지하기 위해 부모 객체 변경
            transform.SetParent(inventoryRectTransform);

            // 기존 UI 위치 기억
            originalUIBeforePosition = rectTransform.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                inventoryRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.localPosition = localPointerPosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 1. 결과 저장용 리스트 생성
            List<RaycastResult> results = new List<RaycastResult>();

            // 2. 현재 마우스 위치에서의 UI 레이캐스트
            EventSystem.current.RaycastAll(eventData, results);

            // 3. 결과 리스트를 순회해서, 원하는 UI 오브젝트 찾기
            foreach (var result in results)
            {
                // 해당 UI의 드래그 종료 이벤트를 실행
                var targetUI = result.gameObject.GetComponent<IDragEndEvent>();
                if (targetUI != null)
                {
                    targetUI.StartDragEndEvent(this);   
                    return;
                }
            }

            transform.SetParent(beforeParent); // 부모 강제 지정
        }
        #endregion

    }
}