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
}
