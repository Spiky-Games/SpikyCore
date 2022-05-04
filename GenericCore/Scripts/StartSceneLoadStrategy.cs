using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoadStrategy : IStartStrategy
{
    public event Action OnLoadFinished;

    private string sceneName;

    public StartSceneLoadStrategy(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public void Load()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(this.sceneName);
        asyncOp.completed += this.OnSceneLoadingFinishedHandler;
    }

    private void OnSceneLoadingFinishedHandler(AsyncOperation op)
    {
        this.OnLoadFinished?.Invoke();
    }
}
