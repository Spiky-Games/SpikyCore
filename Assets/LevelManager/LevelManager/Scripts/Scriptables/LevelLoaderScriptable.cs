using UnityEngine;

public abstract class LevelLoaderScriptable : ScriptableObject
{
    public abstract ILevelLoader LevelLoader
    {
        get;
    }
}
