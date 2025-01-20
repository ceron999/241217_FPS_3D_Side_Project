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
    public List<string> playerNames;

    public System.Action GameStart;
    public System.Action<bool> GameEnd;

    // 킬/데스 액션
    public System.Action PlayerKillAction;
    public System.Action PlayerDieAction;
    public System.Action<int> AIKillAction;
    public System.Action<int> AIDieAction;

    private void Awake()
    {
        playerNames = new List<string>();
        // 임시로 플레이어 이름 지정
        playerNames.Add("Player");

        // 액션 설정
        GameStart += StartGame;
        GameEnd += EndGame;

        PlayerKillAction += PlayerKillActionStart;
        PlayerDieAction += PlayerDieActionStart;
        AIKillAction += AIKillActionStart;
        AIDieAction += AIDieActionStart;

    }

    public void SetStartData(int getAICountData = 1, MainWeaponType getMainWeaponType = MainWeaponType.Rifle)
    {
        StartData = new StartData(getAICountData, getMainWeaponType);
    }

    private void StartGame()
    {
        SituationBoardUI.Instance.InitiateSituationBoardUI(playerNames, StartData.startAICount);
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

    #region 킬/데스 이벤트

    public void PlayerKillActionStart()
    {
        Debug.Log("플레이어 킬");
        SituationBoardUI.Instance.UpdateSituationBoardUI(true, 0, true);
    }

    public void PlayerDieActionStart()
    {
        Debug.Log("플레이어 사망");
        SituationBoardUI.Instance.UpdateSituationBoardUI(true, 0, false);
    }

    public void AIKillActionStart(int characterIndex)
    {
        Debug.Log($"{characterIndex} 킬");
        SituationBoardUI.Instance.UpdateSituationBoardUI(false, characterIndex, true);
    }

    public void AIDieActionStart(int characterIndex)
    {
        Debug.Log($"{characterIndex} 사망");
        SituationBoardUI.Instance.UpdateSituationBoardUI(false, characterIndex, false);
    }
    #endregion
}
