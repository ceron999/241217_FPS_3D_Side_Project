using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : UIBase
{
    public static WeaponUI Instance => UIManager.Singleton.GetUI<WeaponUI>(UIList.WeaponUI);

    public List<Sprite> mainWeaponSprite = new List<Sprite>();

    private void Start()
    {
        if (GameManager.StartData.startMainWeaponType == MainWeaponType.Rifle)
            mainWeaponImage.sprite = mainWeaponSprite[0];
        else
            mainWeaponImage.sprite = mainWeaponSprite[1];
    }

    public override void Show()
    {
        base.Show();

        StartCoroutine(WeaponUIShowCoroutine());
    }

    public Image mainWeaponImage;
    public GameObject grenadeUI;
    public GameObject c4UI;

    // 3ÃÊ µÚ¿¡ ²¨Áü
    IEnumerator WeaponUIShowCoroutine()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }

    public void SetGrenadeUIOff()
    {
        grenadeUI.SetActive(false);
    }

    public void SetC4UIOff()
    {
        c4UI.SetActive(false);
    }
}
