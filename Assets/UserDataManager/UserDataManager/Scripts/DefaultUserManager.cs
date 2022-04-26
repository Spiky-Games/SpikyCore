using EventSystem;
using UnityEngine;

public class DefaultUserManager : MonoBehaviour, IUserManager
{
    private const string USER_DATA_PREF_KEY = "UserData";

    public static DefaultUserManager Instance
    {
        get;
        private set;
    }

    public IUserData UserData
    {
        get;
        set;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.UserData = this.LoadUserData();
        this.Bind();
    }

    private void Bind()
    {
        this.UserData.OnUserDataModified += this.OnUserDataModifiedHandler;
        DefaultGameEventSystemManager.Instance.GameEventSystem.AddEventListener<LevelCompletedEvent>(this.OnLevelCompletedEventHandler);
    }

    private void OnLevelCompletedEventHandler(LevelCompletedEvent levelCompletedEvent)
    {
        this.UserData.CurrentLevel++;
    }

    private void OnUserDataModifiedHandler()
    {
        this.SaveUserData();
    }

    private void SaveUserData()
    {
        PlayerPrefs.SetString(USER_DATA_PREF_KEY, JsonUtility.ToJson(this.UserData));
    }

    private IUserData LoadUserData()
    {
        if (PlayerPrefs.HasKey(USER_DATA_PREF_KEY))
        {
            DefaultUserData savedUserData = JsonUtility.FromJson<DefaultUserData>(PlayerPrefs.GetString(USER_DATA_PREF_KEY));
            return savedUserData;
        }

        DefaultUserData defaultUserData = DefaultUserData.DefaultNewUserData();
        return defaultUserData;
    }
}
