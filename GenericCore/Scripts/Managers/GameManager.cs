using EventSystem;
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

    private void Bind()
    {
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<ForceReloadLevelEvent>(this.ForceReloadLevelEventHandler);
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<FailedPopupClosedEvent>(this.FailedPopupClosedEventHandler);
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<SuccessPopupClosedEvent>(this.SuccessPopupClosedEventHandler);
    }

    public void OpenSuccessPopup()
    {
        UIService.Instance.OpenPopup(PopupType.LevelCompletePopup);
    }

    public void OpenFailedPopup()
    {
        UIService.Instance.OpenPopup(PopupType.LevelFailPopup);
    }

    private void SuccessPopupClosedEventHandler(SuccessPopupClosedEvent successPopupClosedEvent)
    {
        DefaultGameEventSystemManager.Instance.GameEventSystem.Dispatch(new ForceOverrideUserLevelEvent(1));
        DefaultGameEventSystemManager.Instance.GameEventSystem.Dispatch(new ForceReloadLevelEvent());
    }

    private void FailedPopupClosedEventHandler(FailedPopupClosedEvent failedPopupClosedEvent)
    {
        DefaultGameEventSystemManager.Instance.GameEventSystem.Dispatch(new ForceReloadLevelEvent());
    }

    private void ForceReloadLevelEventHandler(ForceReloadLevelEvent forceReloadLevelEvent)
    {
        this.FadeInAndLoadLevel();
    }

    private void StartLevel()
    {
        this.LoadCurrentLevel();
    }

    private void FadeInAndLoadLevel()
    {
        UIService.Instance.StartFadeIn();
        UIService.Instance.OnFadeInCompleted += this.LoadCurrentLevel;
    }

    private void LoadCurrentLevel()
    {
        UIService.Instance.OnFadeInCompleted -= this.LoadCurrentLevel;
        LevelDefinition levelDefinition = DefaultLevelManager.Instance.LoadLevel(DefaultUserManager.Instance.UserData.CurrentLevel);
        SceneManager.LoadScene(levelDefinition.sceneToLoad.name);
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(levelDefinition.sceneToLoad.name);
        asyncOp.completed += this.OnSceneLoadingFinished;
    }

    private void OnSceneLoadingFinished(AsyncOperation obj)
    {
        UIService.Instance.StartFadeOut();
    }
}
