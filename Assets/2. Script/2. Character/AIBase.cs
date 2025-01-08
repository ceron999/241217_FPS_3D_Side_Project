using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : CharacterBase
{

    private void Start()
    {
        nowWeapon = weaponHolder.GetChild(0).GetComponent<WeaponBase>();
    }
}