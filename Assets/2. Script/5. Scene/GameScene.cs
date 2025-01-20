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
        // Ui ¼û±â±â
        UIManager.Hide<CrosshairUI>(UIList.CrosshairUI);
        UIManager.Hide<RaderUI>(UIList.RaderUI);
        UIManager.Hide<GameDataUI>(UIList.GameDataUI);
        UIManager.Hide<StatusUI>(UIList.StatusUI);
        UIManager.Hide<BulletUI>(UIList.BulletUI);
        UIManager.Hide<WeaponUI>(UIList.WeaponUI);

        // UI Á¦°Å
        UIManager.Singleton.GetUI<CrosshairUI>(UIList.CrosshairUI, true);
        UIManager.Singleton.GetUI<RaderUI>(UIList.RaderUI, true);
        UIManager.Singleton.GetUI<GameDataUI>(UIList.GameDataUI, true);
        UIManager.Singleton.GetUI<StatusUI>(UIList.StatusUI, true);
        UIManager.Singleton.GetUI<BulletUI>(UIList.BulletUI, true);
        UIManager.Singleton.GetUI<WeaponUI>(UIList.WeaponUI, true);
        UIManager.Singleton.GetUI<GameEndUI>(UIList.GameEndUI, true);
        UIManager.Singleton.GetUI<ZoomUI>(UIList.ZoomUI, true);
        UIManager.Singleton.GetUI<SituationBoardUI>(UIList.SituationBoardUI, true);

        yield return null;
    }
}
