using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : WeaponBase
{
    protected override void Awake()
    {
        weaponDamage = 40;
    }
    public override bool Activate()
    {
        return true;
    }
}
