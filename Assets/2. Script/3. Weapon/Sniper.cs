using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Gun
{
    protected override void Awake()
    {
        maxAmmo = 40;
        holdAmmo = maxAmmo;
        clipSize = 10;
        currentAmmo = clipSize;

        weaponDamage = 80;
    }
}
