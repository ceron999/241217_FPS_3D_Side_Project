using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : WeaponBase
{
    public Transform throwStartPivot;

    Rigidbody rigid;
    public Vector3 throwVector = Vector3.zero;
    public float throwPower = 7;

    // ���� ���� ����
    public LayerMask targetLayerMask;
    float bombRadius = 2.5f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        maxAmmo = 0;
        holdAmmo = 0;
        clipSize = 1;
        currentAmmo = clipSize;

        weaponDamage = 120;
    }

    public override bool Activate()
    {
        clipSize--;

        // ��ô ���� ����
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
        // 1. ���� �ݰ� ��ü Ž��
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f,
                                                    targetLayerMask, QueryTriggerInteraction.Ignore);

        // 2. ���� ����Ʈ ����
        // Muzzle effect ���
        EffectManager.Instance.CreateEffect(EffectType.MuzzleFlash3, this.transform.position, this.transform.rotation, 10f);

        // 2. ���� ������ ����
        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].transform.root.TryGetComponent(out IDamage damageInterface))
            {
                damageInterface.ApplyDamage(weaponDamage);
            }
        

        // 3. ����
        Debug.Log("Boom");
        Destroy(this.gameObject);
    }
}
