using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : WeaponBase
{
    public Transform installPosition;
    public float curInstallTime = 0f;
    public float maxInstallTime = 5f;

    public float c4ExplosionTIme = 60000000.1f;

    public override bool Activate()
    {
        Debug.Log("C4 설치 시작!");
        curInstallTime = 0f;
        if(this.gameObject.activeSelf)
            StartCoroutine(InstallC4Coroutine());

        return true;
    }

    private IEnumerator InstallC4Coroutine()
    {
        while (curInstallTime < maxInstallTime)
        {
            // 설치가 중단되면 정지
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("C4 설치 중단");
                break;
            }
            curInstallTime += Time.deltaTime;

            yield return null;
        }

        // 설치시간을 충족했다면 설치
        if (curInstallTime >= maxInstallTime)
        {
            Debug.Log("C4 설치");

            // WeaponUI에 C4 끄기
            OldInputSystem.Instance.OnClickAlpha5 = null;

            // C4 객체 생성해서 지정 위치에 놓기
            GameObject go = Instantiate(this.gameObject);
            go.transform.position = installPosition.position;

            // 현재 객체 끄기
            this.gameObject.SetActive(false);

            // 주 무기로 변경
            OldInputSystem.Instance.OnClickAlpha1?.Invoke();
        }
    }
}
