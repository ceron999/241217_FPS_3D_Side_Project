using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    public class InventoryEventUI : MonoBehaviour, IDragEndEvent
    {
        public void StartDragEndEvent(WeaponItemPrefab item)
        {
            item.transform.SetParent(transform);
        }
    }
}