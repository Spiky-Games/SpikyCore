using System;

public interface IStartStrategy
{
    public event Action OnLoadFinished;
    public void Load();
}
