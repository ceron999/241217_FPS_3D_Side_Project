using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : SceneBase
{
    public override bool IsAdditiveScene => false;

    public override IEnumerator OnStart()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneType.StartScene.ToString(), LoadSceneMode);
        while (!async.isDone)
        {
            yield return null;

            float progress = async.progress / 0.9f;
            //LoadingUI.Instance.SetProgress(progress);
        }

        // Show Title UI
        UIManager.Show<StartUI>(UIList.StartUI);

        //SoundManager.Singleton.PlayMusic(MusicFileName.BGM_01);
    }

    public override IEnumerator OnEnd()
    {
        // Hide Title UI
        UIManager.Hide<StartUI>(UIList.StartUI);

        yield return null;
    }
}
