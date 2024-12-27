using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
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
        if(collision != null)
        {
            Debug.Log(collision.gameObject.name);
        }
    }
}
