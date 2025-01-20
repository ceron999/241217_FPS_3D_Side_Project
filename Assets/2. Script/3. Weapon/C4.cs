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
        Debug.Log("C4 ��ġ ����!");
        curInstallTime = 0f;
        if(this.gameObject.activeSelf)
            StartCoroutine(InstallC4Coroutine());

        return true;
    }

    private IEnumerator InstallC4Coroutine()
    {
        while (curInstallTime < maxInstallTime)
        {
            // ��ġ�� �ߴܵǸ� ����
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("C4 ��ġ �ߴ�");
                break;
            }
            curInstallTime += Time.deltaTime;

            yield return null;
        }

        // ��ġ�ð��� �����ߴٸ� ��ġ
        if (curInstallTime >= maxInstallTime)
        {
            Debug.Log("C4 ��ġ");
            // Battle Manager�� C4 ��ġ�ߴٰ� �˸�
            BattleManager.Instance.isC4Install = true;

            // ���ӸŴ������� Ÿ�̸� �缳�� ��û
            GameDataUI.Instance.InitializeTime(c4ExplosionTIme);

            // WeaponUI�� C4 ����
            WeaponUI.Instance.SetC4UIOff();
            InputSystem.Instance.OnClickAlpha5 = null;

            // C4 ��ü �����ؼ� ���� ��ġ�� ����
            GameObject go = Instantiate(this.gameObject);
            go.transform.position = installPosition.position;

            // ���� ��ü ����
            this.gameObject.SetActive(false);
        }
    }
}
