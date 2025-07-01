using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorUI : MonoBehaviour
{
    void Update()
    {
        UpdateIndicator();
    }

    public void UpdateIndicator()
    {
        Quaternion cameraTransform = Camera.main.transform.rotation;

        this.transform.rotation = cameraTransform;
    }
}
