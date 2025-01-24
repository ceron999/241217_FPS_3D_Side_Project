using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeVinetteController : MonoBehaviour
{
    public static ScopeVinetteController Instance { get; private set; } = null;

    public UnityEngine.Rendering.PostProcessing.PostProcessVolume vinetteVolume;

    private float targetWeight = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void SetActiveVinette(bool isActive)
    {
        targetWeight = isActive ? 1f : 0f;
    }

    private void Update()
    {
        vinetteVolume.weight = Mathf.Lerp(vinetteVolume.weight, targetWeight, Time.deltaTime * 10f);
    }
}
