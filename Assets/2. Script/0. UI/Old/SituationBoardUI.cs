using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SituationBoardUI : UIBase
{
    public static SituationBoardUI Instance => UIManager.Singleton.GetUI<SituationBoardUI>(UIList.SituationBoardUI);

    public GameObject InfoPrefab;

    public Transform playerTeamInfoParent;
    public Transform aiTeamNameParent;

    private Dictionary<int, InfoPrefab> playerInfoDictionary = new Dictionary<int, InfoPrefab>();
    private Dictionary<int, InfoPrefab> aiInfoDictionary = new Dictionary<int, InfoPrefab>();
    public void InitiateSituationBoardUI(List<string> playerNameList, int aiCount)
    {
        if (playerInfoDictionary.Count > 0)
        {
            for (int i = 0; i < playerInfoDictionary.Count; i++)
            {
                Destroy(playerInfoDictionary[i].gameObject);
                playerInfoDictionary.Remove(i);
            }
        }
        if (aiInfoDictionary.Count > 0)
        {
            for (int i = 0; i < aiInfoDictionary.Count; i++)
            {
                Destroy(aiInfoDictionary[i].gameObject);
                aiInfoDictionary.Remove(i);
            }
        }

        for (int i = 0; i < playerNameList.Count; i++)
        {
            InfoPrefab goPrefab = Instantiate(InfoPrefab).GetComponent<InfoPrefab>();
            goPrefab.transform.SetParent(playerTeamInfoParent);
            goPrefab.SetInfoPrefab(playerNameList[i], 0);
            playerInfoDictionary[i] = goPrefab;
        }

        for (int i = 0; i < aiCount; i++)
        {
            InfoPrefab goPrefab = Instantiate(InfoPrefab).GetComponent<InfoPrefab>();
            goPrefab.transform.SetParent(aiTeamNameParent);
            goPrefab.SetInfoPrefab($"AI {i}", 0);
            aiInfoDictionary[i] = goPrefab;
        }
    }

    public void UpdateSituationBoardUI(bool isPlayer, int dictionaryKey, bool isKill)
    {
        // 플레이어 쪽 업데이트
        if(isPlayer)
        {
            if(isKill)
                playerInfoDictionary[dictionaryKey].UpdateKillCount();
            else
                playerInfoDictionary[dictionaryKey].UpdateDead();
        }

        // AI 쪽 업데이트
        else
        {
            if (isKill)
                aiInfoDictionary[dictionaryKey].UpdateKillCount();
            else
                aiInfoDictionary[dictionaryKey].UpdateDead();
        }
    }

    public int GetPlayerKillCount(int playerIndex)
    {
        return playerInfoDictionary[playerIndex].GetKillCount();
    }
}
