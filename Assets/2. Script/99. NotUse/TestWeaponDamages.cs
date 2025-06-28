using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotUsed
{
    public class TestWeaponDamages : MonoBehaviour, IDamage
    {
        float hp = 100;
        public void ApplyDamage(float getDamage)
        {
            if (hp - getDamage > 0)
            {
                hp -= getDamage;
                Debug.Log($"{getDamage}만큼 데미지를 받았습니다");
            }
            else
                Destroy(this.gameObject);
        }
    }
}