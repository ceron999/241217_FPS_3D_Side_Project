using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem Instance;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Camera.main.transform.position, AimingPoint);
    }

    public Cinemachine.CinemachineVirtualCamera tpsCamera;

    public Transform follow;
    public Transform target;

    public Vector3 AimingPoint;
    public LayerMask aimingLayerMask;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Update()
    {
        // 에임 포인트 잡기
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, aimingLayerMask, QueryTriggerInteraction.Ignore))
        {
            AimingPoint = hitInfo.point;
        }
        else
        {
            AimingPoint = ray.GetPoint(100f);
        }
    }

    public void SetCameraFollowTarget(Transform cameraPivot)
    {
        tpsCamera.Follow = cameraPivot;
    }
}
