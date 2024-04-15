using EventSystem;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnLevelLoad;

    private const string CURRENT_LEVEL_KEY = "CurrentLevel";

    public int CurrentLevel
    {
        get { return PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 0); }
        set { PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, value); }
    }

    public static GameManager Instance
    {
        get;
        set;
    }

    public IStartStrategy startStrategy;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    protected virtual void Start()
    {
        this.Bind();
        this.StartLevel();
    }

    protected void Bind()
    {
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<LevelCompletedEvent>(this.OnLevelCompletedEventHandler);
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<ForceOverrideUserLevelEvent>(this.OnForceOverrideUserLevelEventHandler);
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<ForceReloadLevelEvent>(this.ForceReloadLevelEventHandler);
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<FailedPopupClosedEvent>(this.FailedPopupClosedEventHandler);
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<SuccessPopupClosedEvent>(this.SuccessPopupClosedEventHandler);
    }

    private void OnForceOverrideUserLevelEventHandler(ForceOverrideUserLevelEvent forceOverrideUserLevelEvent)
    {
        int newLevel = this.CurrentLevel;
        newLevel += forceOverrideUserLevelEvent.LevelsToModify;
        newLevel = Mathf.Max(0, newLevel);
        this.CurrentLevel = newLevel;
    }

    private void OnLevelCompletedEventHandler(LevelCompletedEvent levelCompletedEvent)
    {
        this.CurrentLevel++;
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

    protected void StartLevel()
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
        LevelDefinition levelDefinition = DefaultLevelManager.Instance.LoadLevel(this.CurrentLevel);
        this.startStrategy = new StartSceneLoadStrategy(levelDefinition.sceneToLoad);
        this.startStrategy.Load();
        this.startStrategy.OnLoadFinished += this.OnSceneLoadingFinished;
    }

    private void OnSceneLoadingFinished()
    {
        OnLevelLoad?.Invoke();
        this.startStrategy.OnLoadFinished -= this.OnSceneLoadingFinished;
        UIService.Instance.StartFadeOut();
    }
}
