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
        this.LoadCurrentLevel();
    }

    private void StartLevel()
    {
        UIService.Instance.OnFadeOutCompleted += this.OnFadeOutPreLoadCurrentLevelCompleted;
        UIService.Instance.StartFadeOut();
    }

    private void OnFadeOutPreLoadCurrentLevelCompleted()
    {
        UIService.Instance.OnFadeOutCompleted -= this.OnFadeOutPreLoadCurrentLevelCompleted;
        this.LoadCurrentLevel();
    }

    private void LoadCurrentLevel()
    {
        UIService.Instance.OnFadeInCompleted += this.OnFadeOutLoadCompletedHandler;
        UIService.Instance.StartFadeIn();
    }

    private void OnFadeOutLoadCompletedHandler()
    {
        UIService.Instance.OnFadeInCompleted -= this.OnFadeOutLoadCompletedHandler;
        LevelDefinition levelDefinition = DefaultLevelManager.Instance.LoadLevel(DefaultUserManager.Instance.UserData.CurrentLevel);
        SceneManager.LoadScene(levelDefinition.sceneToLoad.name);
    }
}
