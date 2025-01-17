using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StartData
{
    public int startAICount;
    public MainWeaponType startMainWeaponType;

    public StartData(int getAICountData  = 1 , MainWeaponType getMainWeaponType = MainWeaponType.Rifle)
    {
        startAICount = getAICountData;
        startMainWeaponType = getMainWeaponType;
    }
}

public class GameManager : SingletonBase<GameManager>
{
    public static StartData StartData;     // ���� ȭ�鿡�� ���� ���� ��ư�� �÷��� �� ���

    public void SetStartData(int getAICountData = 1, MainWeaponType getMainWeaponType = MainWeaponType.Rifle)
    {
        StartData = new StartData(getAICountData, getMainWeaponType);
    }
}
