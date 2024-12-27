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
        rigid.AddForce(transform.up * (-1f) * bulletSpeed, ForceMode.Impulse);
        Destroy(gameObject, lifeTime); // 본인 GameObject를 LifeTime 이후에 파괴 되도록 명령
    }
}
