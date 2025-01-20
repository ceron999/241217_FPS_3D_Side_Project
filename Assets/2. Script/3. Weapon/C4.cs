using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : WeaponBase
{
    public Transform installPosition;
    public float curInstallTime = 0f;
    public float maxInstallTime = 5f;

    public float c4ExplosionTIme = 60.1f;

    protected override void Awake()
    {
        weaponDamage = 10000;
    }

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
            // Battle Manager에 C4 설치했다고 알림
            BattleManager.Instance.isC4Install = true;

            // 게임매니저한테 타이머 재설정 요청
            GameDataUI.Instance.InitializeTime(c4ExplosionTIme);

            // WeaponUI에 C4 끄기
            WeaponUI.Instance.SetC4UIOff();
            InputSystem.Instance.OnClickAlpha5 = null;

            // C4 객체 생성해서 지정 위치에 놓기
            GameObject go = Instantiate(this.gameObject);
            go.transform.position = installPosition.position;

            // 현재 객체 끄기
            this.gameObject.SetActive(false);
        }
    }
}
