using System;

public interface IUserData
{
    public event Action OnUserDataModified;

    public int CurrentLevel
    {
        get;
        set;
    }
}
