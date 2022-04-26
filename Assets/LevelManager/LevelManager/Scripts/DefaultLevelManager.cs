using EventSystem;
using UnityEngine;

public class DefaultLevelManager : MonoBehaviour, ILevelManager
{
    [SerializeField]
    private LevelLoaderScriptable levelLoaderScriptable;

    public static DefaultLevelManager Instance
    {
        get;
        set;
    }

    private void Awake()
    {
        Instance = this;
    }

    public LevelDefinition LoadLevel(int levelIndex)
    {
        LevelDefinition levelDefinition = this.levelLoaderScriptable.LevelLoader.Load(levelIndex);
        DefaultGameEventSystemManager.Instance.GameEventSystem.Dispatch(new LevelLoadedEvent());
        return levelDefinition;
    }

    public void CompleteLevel()
    {
        DefaultGameEventSystemManager.Instance.GameEventSystem.Dispatch(new LevelCompletedEvent());
    }
}
