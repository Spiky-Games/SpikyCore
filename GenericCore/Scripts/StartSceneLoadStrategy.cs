using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoadStrategy : IStartStrategy
{
    public event Action OnLoadFinished;

    private SceneAsset assetScene;

    public StartSceneLoadStrategy(SceneAsset assetScene)
    {
        this.assetScene = assetScene;
    }

    public void Load()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(this.assetScene.name);
        asyncOp.completed += this.OnSceneLoadingFinishedHandler;
    }

    private void OnSceneLoadingFinishedHandler(AsyncOperation op)
    {
        this.OnLoadFinished?.Invoke();
    }
}
