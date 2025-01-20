using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : WeaponBase
{
    public Transform installPosition;
    public float curInstallTime = 0f;
    public float maxInstallTime = 5f;

    public float c4ExplosionTIme = 10f;

    protected override void Awake()
    {
        weaponDamage = 10000;
    }

    public override bool Activate()
    {
        Debug.Log("C4 설치 시작!");
        curInstallTime = 0f;
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
            BattleManager.Instance.isC4Install = true;
            GameDataUI.Instance.InitializeTime(c4ExplosionTIme);

            GameObject go = Instantiate(this.gameObject);
            go.transform.position = installPosition.position;
        }
    }
}
