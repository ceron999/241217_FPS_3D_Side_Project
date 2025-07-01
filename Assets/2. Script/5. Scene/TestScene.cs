using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScene : SceneBase
{
    public override bool IsAdditiveScene => false;

    public override IEnumerator OnStart()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneType.GameScene.ToString(), LoadSceneMode);
        while (!async.isDone)
        {
            yield return null;

            float progress = async.progress / 0.9f;
            //LoadingUI.Instance.SetProgress(progress);
        }

        UIManager.Show<CrosshairUI>(UIList.CrosshairUI);
        UIManager.Show<InventoryUI>(UIList.InventoryUI);
        UIManager.Hide<InventoryUI>(UIList.InventoryUI);
    }

    public override IEnumerator OnEnd()
    {
        // Ui ¼û±â±â
        UIManager.Hide<CrosshairUI>(UIList.CrosshairUI);
        UIManager.Hide<InventoryUI>(UIList.InventoryUI);

        // UI Á¦°Å
        UIManager.Singleton.GetUI<CrosshairUI>(UIList.CrosshairUI, true);
        UIManager.Singleton.GetUI<InventoryUI>(UIList.InventoryUI, true);

        yield return null;
    }
}
