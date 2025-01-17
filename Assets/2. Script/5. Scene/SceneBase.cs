using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneBase : MonoBehaviour
{
    public LoadSceneMode LoadSceneMode => IsAdditiveScene ? LoadSceneMode.Additive : LoadSceneMode.Single;
    public abstract bool IsAdditiveScene { get; }
    public abstract IEnumerator OnStart();
    public abstract IEnumerator OnEnd();
}
