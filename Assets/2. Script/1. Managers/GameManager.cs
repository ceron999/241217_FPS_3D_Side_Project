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
    public static StartData StartData;     // 시작 화면에서 게임 시작 버튼을 늘렀을 떄 사용

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

        // 일시정지
        Time.timeScale = 0f;

        // 커서 활성화
        InputSystem.Instance.isStopCameraMove = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameEndUI.Instance.Show();
        GameEndUI.Instance.SetGameEndText(isWin);
    }
}
