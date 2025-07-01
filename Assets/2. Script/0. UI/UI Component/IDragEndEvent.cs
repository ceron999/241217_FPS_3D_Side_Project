using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    public interface IDragEndEvent
    {
        /// <summary>
        /// 드래그 이벤트가 종료되었을 때 실행할 함수
        /// </summary>
        void StartDragEndEvent(WeaponItemPrefab item);
    }
}