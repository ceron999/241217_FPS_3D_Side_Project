using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    MuzzleFlash1, MuzzleFlash2, MuzzleFlash3, MuzzleFlash4, MuzzleFlash5, 
    MuzzleFlash6, MuzzleFlash7, MuzzleFlash8, MuzzleFlash9, MuzzleFlash10, 

}

[System.Serializable]
public class EffectData
{
    public EffectType effectType;
    public GameObject effectPrefab;
    public float lifeTime;
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; } = null;

    public List<EffectData> effectDataList = new List<EffectData>();

    private void Awake()
    {
        Instance = this;
    }

    public void CreateEffect(EffectType type, Vector3 position, Quaternion rotation)
    {
        var targetEffectData = effectDataList.Find(x => x.effectType == type);
        if (targetEffectData == null)
            return;

        var newEffect = Instantiate(targetEffectData.effectPrefab, position, rotation);
        newEffect.gameObject.SetActive(true);
        Destroy(newEffect, targetEffectData.lifeTime);
    }
}
