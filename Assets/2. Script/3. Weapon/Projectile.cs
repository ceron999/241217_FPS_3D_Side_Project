using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletDamage;
    public float lifeTime;
    public float bulletSpeed;

    public System.Action returnToPoolCallBack;

    private void Start()
    {
        //Invoke(returnToPoolCallBack?.Invoke(), lifeTime); // ���� GameObject�� LifeTime ���Ŀ� �ı� �ǵ��� ���
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
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
    }
    
}
