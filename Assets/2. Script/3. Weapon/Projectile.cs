using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 firePoint;
    public float bulletDamage;
    
    public float bulletSpeed;

    [Header("pool 복귀 타이밍 조절 데이터")]
    private DateTime shootingTime;
    private DateTime returnTime;
    public float lifeTime;

    [SerializeField] private Rigidbody bulletRb;

    public System.Action returnToPoolCallBack;

    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        shootingTime = DateTime.Now;
        returnTime = shootingTime.AddSeconds(lifeTime);


        Vector3 shootDir = (CameraSystem.Instance.AimingPoint - firePoint).normalized;
        
        bulletRb.velocity = shootDir * bulletSpeed;
    }

    private void Update()
    {
        if (returnTime < DateTime.Now)
            returnToPoolCallBack?.Invoke();
    }

    private void OnDisable()
    {
        shootingTime = DateTime.MaxValue;
        returnTime = DateTime.MaxValue;
    }

    public void Init(System.Action onReturn)
    {
        returnToPoolCallBack = onReturn;
    }

    private void OnTriggerEnter(Collider getCol)
    {
        if (getCol.gameObject.layer == LayerMask.NameToLayer("HitScanner"))
        {
            if (getCol.transform.root.TryGetComponent(out IDamage damageInterface))
            {
                float damageMultiple = 1f;
                if (getCol.gameObject.TryGetComponent(out DamageMultiflier multiflier))
                {
                    damageMultiple = multiflier.DamageMultiplier;
                }

                damageInterface.ApplyDamage(bulletDamage * damageMultiple);
                //Destroy(this.gameObject);
                returnToPoolCallBack?.Invoke();
            }
        }
        else if(getCol.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            returnToPoolCallBack?.Invoke();
        }
    }
    
}
