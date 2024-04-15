using System;

public class StartPrefabInstantiateStrategy : IStartStrategy
{
    public float Progress => throw new NotImplementedException();

    public event Action OnLoadFinished;

    public void Load()
    {
        throw new NotImplementedException();
    }
}
