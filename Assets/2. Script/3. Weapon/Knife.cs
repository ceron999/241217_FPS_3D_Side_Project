using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : WeaponBase
{
    private void Start()
    {
        weaponDamage = 40;
    }
    public override bool Activate()
    {
        return true;
    }
}
