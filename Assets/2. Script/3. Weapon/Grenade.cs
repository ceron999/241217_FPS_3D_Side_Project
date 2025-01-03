using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : WeaponBase
{
    public Transform throwStartPivot;

    Rigidbody rigid;
    public Vector3 throwVector = Vector3.zero;
    public float throwPower = 10;

    // Æø¹ß °ü·Ã º¯¼ö
    public LayerMask targetLayerMask;
    float bombRadius = 2.5f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        maxAmmo = 1;
        holdAmmo = maxAmmo;
        clipSize = 1;
        currentAmmo = clipSize;

        weaponDamage = 120;
    }

    public override bool Activate()
    {
        throwVector = (throwStartPivot.transform.forward + throwStartPivot.transform.up) * throwPower;
        rigid.AddForce(throwVector, ForceMode.Impulse);
        Boom();
        return true;
    }

    void Boom()
    {
        StartCoroutine(BoomCorontine());
    }

    IEnumerator BoomCorontine()
    {
        yield return new WaitForSeconds(4);
        // 1. Æø¹ß ¹Ý°æ °´Ã¼ Å½Áö
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f,
                                                    targetLayerMask, QueryTriggerInteraction.Ignore);

        // 2. Æø¹ß ÀÌÆåÆ® Àû¿ë
        // Muzzle effect Ãâ·Â
        EffectManager.Instance.CreateEffect(EffectType.MuzzleFlash3, this.transform.position, this.transform.rotation);

        // 2. Æø¹ß µ¥¹ÌÁö Àû¿ë
        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].transform.root.TryGetComponent(out IDamage damageInterface))
            {
                damageInterface.ApplyDamage(weaponDamage);
            }
        

        // 3. Á¦°Å
        Debug.Log("Boom");
        Destroy(this.gameObject);
    }
}
