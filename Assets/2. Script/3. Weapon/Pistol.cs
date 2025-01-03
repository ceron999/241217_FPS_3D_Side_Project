using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    private void Start()
    {
        maxAmmo = 48;
        holdAmmo = maxAmmo;
        clipSize = 12;
        currentAmmo = clipSize;

        weaponDamage = 15;
    }
}
