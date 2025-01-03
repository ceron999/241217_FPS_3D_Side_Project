using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Gun
{
    private void Start()
    {
        maxAmmo = 40;
        holdAmmo = maxAmmo;
        clipSize = 10;
        currentAmmo = clipSize;

        weaponDamage = 80;
    }
}
