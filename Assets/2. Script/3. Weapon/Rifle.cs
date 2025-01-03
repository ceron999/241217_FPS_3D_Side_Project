using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    private void Start()
    {
        maxAmmo = 120;
        holdAmmo = maxAmmo;
        clipSize = 40;
        currentAmmo = clipSize;

        weaponDamage = 20;
    }
}
