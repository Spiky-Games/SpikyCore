using System;

public class DefaultUserData : IUserData
{
    public event Action OnUserDataModified;

    public int CurrentLevel
    {
        get { return this.LoadCurrentLevel(); }
        set { this.SaveCurrentLevel(value); }
    }

    private int currentLevel;

    public static DefaultUserData DefaultNewUserData()
    {
        return new DefaultUserData(0);
    }

    public DefaultUserData(int currentLevel)
    {
        this.CurrentLevel = currentLevel;
    }

    public DefaultUserData()
    {
    }

    private int LoadCurrentLevel()
    {
        return this.currentLevel;
    }

    private void SaveCurrentLevel(int newLevel)
    {
        this.currentLevel = newLevel;
        this.OnUserDataModified?.Invoke();
    }
}
