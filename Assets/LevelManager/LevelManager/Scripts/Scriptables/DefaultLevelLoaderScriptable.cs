using UnityEngine;

[CreateAssetMenu(fileName = "LevelLoaderScriptable", menuName = "CreateUtils/Default Level Loader Scriptable")]
public class DefaultLevelLoaderScriptable : LevelLoaderScriptable
{
    [SerializeField]
    private LevelDefinitionScriptable levelDefinitionScriptable;

    public override ILevelLoader LevelLoader
    {
        get { return this.defaultLevelLoader ??= new DefaultLevelLoader(this.levelDefinitionScriptable); }
    }

    private DefaultLevelLoader defaultLevelLoader;
}
