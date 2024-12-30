using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : UIBase
{
    public override void Show()
    {
        base.Show();

        StartCoroutine(WeaponUIShowCoroutine());
    }

    // 3�� �ڿ� ����
    IEnumerator WeaponUIShowCoroutine()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
