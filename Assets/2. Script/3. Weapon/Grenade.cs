using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : WeaponBase
{
    Rigidbody rigid;
    Vector3 throwVector;
    float throwPower = 3;

    private void Awake()
    {
        //rigid = GetComponent<Rigidbody>();
        //rigid.useGravity = false;
    }

    public override bool Activate()
    {
        //rigid.useGravity = true;
        //rigid.AddForce(throwVector, ForceMode.Impulse);
        return true;
    }
}
