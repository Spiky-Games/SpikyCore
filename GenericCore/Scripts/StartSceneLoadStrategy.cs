using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoadStrategy : IStartStrategy
{
    public float Progress => asOP != null ? asOP.progress : 0;

    public event Action OnLoadFinished;

    private string sceneName;
    AsyncOperation asOP;
    public StartSceneLoadStrategy(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public void Load()
    {
        asOP = SceneManager.LoadSceneAsync(this.sceneName);
        asOP.completed += this.OnSceneLoadingFinishedHandler;
    }

    private void OnSceneLoadingFinishedHandler(AsyncOperation op)
    {
        this.OnLoadFinished?.Invoke();
    }
}
