using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : SceneBase
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
        UIManager.Show<RaderUI>(UIList.RaderUI);
        UIManager.Show<GameDataUI>(UIList.GameDataUI);
        UIManager.Show<StatusUI>(UIList.StatusUI);
        UIManager.Show<BulletUI>(UIList.BulletUI);
        UIManager.Show<WeaponUI>(UIList.WeaponUI);
    }

    public override IEnumerator OnEnd()
    {
        // TODO : Hide Ingame UI
        UIManager.Hide<CrosshairUI>(UIList.CrosshairUI);
        UIManager.Hide<RaderUI>(UIList.RaderUI);
        UIManager.Hide<GameDataUI>(UIList.GameDataUI);
        UIManager.Hide<StatusUI>(UIList.StatusUI);
        UIManager.Hide<BulletUI>(UIList.BulletUI);
        UIManager.Hide<WeaponUI>(UIList.WeaponUI);

        yield return null;
    }
}
