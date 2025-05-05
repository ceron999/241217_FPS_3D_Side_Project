using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : WeaponBase
{
    // �Ѿ� �߻� ���� Ȯ�ο�
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * 100f);
    }

    public Projectile bulletPrefab;
    public Transform firePoint;

    public float fireRate; // ���� �ӵ� (�ð� ��) => ex) 0.1: 0.1�ʿ� 1�߾� �߻� �� �� �ִ� ��

    private float lastFireTime; // ������ �߻� ���� �ð�

    #region Bullet Pool
    public IObjectPool<Projectile> bulletPool;

    [Header("Ǯ�� ������")]
    [SerializeField] int defaultSize;
    [SerializeField] int maxPoolSize;

    #endregion Bullet Pool

    protected override void Awake()
    {
        fireRate = Mathf.Max(fireRate, 0.1f); // ���� �ӵ��� 0.1���� �۴ٸ�, 0.1�� ����
    }

    private void Start()
    {

        InitPool();
    }

    //�� �߻�
    public override bool Activate()
    {
        if (currentAmmo <= 0 || Time.time - lastFireTime < fireRate)
            return false;

        Shoot();
        lastFireTime = Time.time;
        currentAmmo--;

        // Muzzle effect ���
        EffectManager.Instance.CreateEffect(EffectType.MuzzleFlash1, firePoint.position, firePoint.rotation);

        BulletUI.Instance.UpdateAmmoCount(currentAmmo, holdAmmo);

        return true;
    }

    #region Pool Func
    private void InitPool()
    {
        bulletPool = new ObjectPool<Projectile>
            (
                createFunc: CreatePooledItem,
                actionOnGet: OnTakeFromPool,
                actionOnRelease: OnReturnedToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: defaultSize,
                maxSize: maxPoolSize
            );

    }

    Projectile CreatePooledItem()
    {
        Projectile bullet = Instantiate(bulletPrefab);
        bullet.firePoint = firePoint.position;
        return bullet;
    }

    void OnReturnedToPool(Projectile bullet)
    {
        bullet.transform.position = firePoint.position;
        bullet.gameObject.SetActive(false);
    }
    
    void OnTakeFromPool(Projectile bullet)
    {
        bullet.transform.rotation = firePoint.rotation;
        bullet.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(Projectile bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void Shoot()
    {
        Projectile b = bulletPool.Get();
        b.transform.position = firePoint.position;
        b.Init(() => bulletPool.Release(b));  // bullet ���ο��� ��� �� �ݳ�
    }
    #endregion
}
