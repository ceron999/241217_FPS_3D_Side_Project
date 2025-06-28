using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance;

        [Header("무기 데이터")]
        [SerializeField] private List<WeaponData> allWeaponDatas = new List<WeaponData>();
        [SerializeField] private List<WeaponBase> equipWeaponDatas = new List<WeaponBase>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }


    }
}