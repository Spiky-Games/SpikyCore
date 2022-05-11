using EventSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        set;
    }

    private IStartStrategy startStrategy;

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
        UIService.Instance.OpenPopup("LevelCompletePopup");
    }

    public void OpenFailedPopup()
    {
        UIService.Instance.OpenPopup("LevelFailPopup");
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
        this.startStrategy = new StartSceneLoadStrategy(levelDefinition.sceneToLoad);
        this.startStrategy.Load();
        this.startStrategy.OnLoadFinished += this.OnSceneLoadingFinished;
    }

    private void OnSceneLoadingFinished()
    {
        this.startStrategy.OnLoadFinished -= this.OnSceneLoadingFinished;
        UIService.Instance.StartFadeOut();
    }
}
