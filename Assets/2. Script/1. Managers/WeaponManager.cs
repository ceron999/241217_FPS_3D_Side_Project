using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class WeaponManager : SingletonBase<WeaponManager>
    {
        [Header("무기 데이터")]
        [SerializeField] private List<WeaponData> allWeaponDatas = new List<WeaponData>();
    }
}