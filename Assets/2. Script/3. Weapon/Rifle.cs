using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    protected override void Awake()
    {
        maxAmmo = 120;
        holdAmmo = maxAmmo;
        clipSize = 40;
        currentAmmo = clipSize;

        weaponDamage = 20;
    }
}
