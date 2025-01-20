using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public System.Action SpawnAI;               // AI Spawn

    [Header("AI 데이터 관련")]
    public Transform aiSpawnPositionParent;
    public GameObject enemyPrefab;              // 움직이는 객체는 묶어서 사용하지 말 것 
    public Transform aiPatrolPoints;

    [Header("GameDataUI 데이터 관련")]
    public int playerCount;
    public int aiCount;
    public System.Action UpdateGameDataUI;

    private float gameTime  = 180f;
    private float nowTime = 0f;
    private float c4Time = 60f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerCount = 1;
        aiCount = GameManager.StartData.startAICount;

        SpawnAI += SpawnAIPrefab;

        GameDataUI.Instance.InitializeTime(gameTime);
    }

    private void Start()
    {
        SpawnAI?.Invoke();
        UpdateGameDataUIs();
    }

    private void SpawnAIPrefab()
    {
        int aiCount = GameManager.StartData.startAICount;

        for (int i =0; i< aiCount; i++ )
        {
            GameObject goEnemy = Instantiate(enemyPrefab);
            goEnemy.transform.position = aiSpawnPositionParent.GetChild(i).position;

            goEnemy.GetComponent<CharacterController_AI>().patrolPointParent = aiPatrolPoints;
            goEnemy.GetComponent<CharacterBase>().characterIndex = i;
        }
    }
    #region GameDataUI 설정 함수들
    public void PlayerDie()
    {
        playerCount--;
        UpdateGameDataUIs();

        if (playerCount == 0)
        {
            Debug.Log("Game Over");
            GameManager.Singleton.GameEnd?.Invoke(false);
        }
    }

    public void AIDie()
    {
        aiCount--;
        UpdateGameDataUIs();

        if (aiCount == 0)
        {
            Debug.Log("Game Win");
            GameManager.Singleton.GameEnd?.Invoke(true);
        }
    }

    // 플레이어, 적 숫자 표기
    private void UpdateGameDataUIs()
    {
        GameDataUI.Instance.UpdateGameData(playerCount, aiCount);
    }

    public void InstallC4()
    {
        GameDataUI.Instance.InitializeTime(c4Time);
    }
    #endregion
}
