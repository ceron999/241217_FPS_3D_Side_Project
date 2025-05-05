using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : WeaponBase
{
    // 총알 발사 궤적 확인용
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * 100f);
    }

    public Projectile bulletPrefab;
    public Transform firePoint;

    public float fireRate; // 연사 속도 (시간 값) => ex) 0.1: 0.1초에 1발씩 발사 할 수 있는 값

    private float lastFireTime; // 마지막 발사 실제 시간

    #region Bullet Pool
    public IObjectPool<Projectile> bulletPool;

    [Header("풀링 데이터")]
    [SerializeField] int defaultSize;
    [SerializeField] int maxPoolSize;

    #endregion Bullet Pool

    protected override void Awake()
    {
        fireRate = Mathf.Max(fireRate, 0.1f); // 연사 속도가 0.1보다 작다면, 0.1로 설정
    }

    private void Start()
    {

        InitPool();
    }

    //총 발사
    public override bool Activate()
    {
        if (currentAmmo <= 0 || Time.time - lastFireTime < fireRate)
            return false;

        Shoot();
        lastFireTime = Time.time;
        currentAmmo--;

        // Muzzle effect 출력
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
        b.Init(() => bulletPool.Release(b));  // bullet 내부에서 사용 후 반납
    }
    #endregion
}
