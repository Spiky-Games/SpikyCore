using System;

public class StartPrefabInstantiateStrategy : IStartStrategy
{
    public event Action OnLoadFinished;

    public void Load()
    {
        throw new NotImplementedException();
    }
}
