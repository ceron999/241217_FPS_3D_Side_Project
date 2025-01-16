using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : WeaponBase
{
    protected override void Awake()
    {
        weaponDamage = 10000;
    }
    public override bool Activate()
    {
        return true;
    }
}
