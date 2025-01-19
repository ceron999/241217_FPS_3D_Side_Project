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

    public System.Action GameStart;
    public System.Action<bool> GameEnd;

    private void Awake()
    {
        GameStart += StartGame;
        GameEnd += EndGame;
    }

    public void SetStartData(int getAICountData = 1, MainWeaponType getMainWeaponType = MainWeaponType.Rifle)
    {
        StartData = new StartData(getAICountData, getMainWeaponType);
    }

    private void StartGame()
    {
        Time.timeScale = 1f;
    }

    private void EndGame(bool isWin)
    {
        StartCoroutine(EndGameCoroutine(isWin));
    }

    private IEnumerator EndGameCoroutine(bool isWin)
    {
        yield return new WaitForSeconds(3f);

        // �Ͻ�����
        Time.timeScale = 0f;

        // Ŀ�� Ȱ��ȭ
        InputSystem.Instance.isStopCameraMove = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameEndUI.Instance.Show();
        GameEndUI.Instance.SetGameEndText(isWin);
    }
}
