using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        set;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        this.Bind();
        this.StartLevel();
    }

    private void OnDestroy()
    {
        this.Unbind();
    }

    private void Bind()
    {
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<ForceReloadLevelEvent>(this.ForceReloadLevelEventHandler);
    }

    private void Unbind()
    {
        if (DefaultGameEventSystemManager.Instance != null)
        {
            DefaultGameEventSystemManager.Instance.GameEventSystem.RemoveEventListener<ForceReloadLevelEvent>(this.ForceReloadLevelEventHandler);
        }
    }

    private void ForceReloadLevelEventHandler(ForceReloadLevelEvent forceReloadLevelEvent)
    {
        this.LoadCurrentLevel();
    }

    private void StartLevel()
    {
        //this.LoadCurrentLevel();
    }

    private void LoadCurrentLevel()
    {
        LevelDefinition levelDefinition = DefaultLevelManager.Instance.LoadLevel(DefaultUserManager.Instance.UserData.CurrentLevel);
        SceneManager.LoadScene(levelDefinition.sceneToLoad.name);
    }
}
