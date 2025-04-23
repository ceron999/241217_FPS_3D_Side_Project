using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        InitPool();
    }

    int defaultSIze = 30;
    int maxPoolSize = 100;
    public GameObject smallProjectilePrefab;
    public GameObject largeProjectilePrefab;

    public IObjectPool<GameObject> IProjectilePool;

    public void InitPool()
    {
        IProjectilePool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, defaultSIze, maxPoolSize);

    }

    // ����
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(smallProjectilePrefab);
        return poolGo;
    }

    // ���
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
}
