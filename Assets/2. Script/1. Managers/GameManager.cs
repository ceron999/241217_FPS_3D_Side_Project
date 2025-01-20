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
    public List<string> playerNames;

    public System.Action GameStart;
    public System.Action<bool> GameEnd;

    // ų/���� �׼�
    public System.Action PlayerKillAction;
    public System.Action PlayerDieAction;
    public System.Action<int> AIKillAction;
    public System.Action<int> AIDieAction;

    private void Awake()
    {
        playerNames = new List<string>();
        // �ӽ÷� �÷��̾� �̸� ����
        playerNames.Add("Player");

        // �׼� ����
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

        // �Ͻ�����
        Time.timeScale = 0f;

        // Ŀ�� Ȱ��ȭ
        InputSystem.Instance.isStopCameraMove = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameEndUI.Instance.Show();
        GameEndUI.Instance.SetGameEndText(isWin);
    }

    #region ų/���� �̺�Ʈ

    public void PlayerKillActionStart()
    {
        Debug.Log("�÷��̾� ų");
        SituationBoardUI.Instance.UpdateSituationBoardUI(true, 0, true);
    }

    public void PlayerDieActionStart()
    {
        Debug.Log("�÷��̾� ���");
        SituationBoardUI.Instance.UpdateSituationBoardUI(true, 0, false);
    }

    public void AIKillActionStart(int characterIndex)
    {
        Debug.Log($"{characterIndex} ų");
        SituationBoardUI.Instance.UpdateSituationBoardUI(false, characterIndex, true);
    }

    public void AIDieActionStart(int characterIndex)
    {
        Debug.Log($"{characterIndex} ���");
        SituationBoardUI.Instance.UpdateSituationBoardUI(false, characterIndex, false);
    }
    #endregion
}
