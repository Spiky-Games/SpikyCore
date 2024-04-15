using System;

public interface IStartStrategy
{
    public float Progress
    { get;}
    public event Action OnLoadFinished;
    public void Load();
}
