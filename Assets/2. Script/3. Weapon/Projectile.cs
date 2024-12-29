using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletDamage;
    public float lifeTime;
    public float bulletSpeed;

    private void Start()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(gameObject, lifeTime); // ���� GameObject�� LifeTime ���Ŀ� �ı� �ǵ��� ���
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Debug.Log(collision.gameObject.name);
            if(collision.gameObject.TryGetComponent(out IDamage damageInterface))
            {
                float damageMultiple = 1f;
                if(collision.gameObject.TryGetComponent(out DamageMultiflier multiflier))
                {
                    damageMultiple = multiflier.DamageMultiplier;
                }
                damageInterface.ApplyDamage(bulletDamage * damageMultiple);
            }
        }
    }
}
