using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultLevelLoader : ILevelLoader
{
    private LevelDefinitionScriptable levelDefinitionScriptable;

    public DefaultLevelLoader(LevelDefinitionScriptable levelDefinitionScriptable)
    {
        this.levelDefinitionScriptable = levelDefinitionScriptable;
    }

    public LevelDefinition Load(int levelToLoad)
    {
        int maxLevels = SceneManager.sceneCountInBuildSettings - 1;
        int round = Mathf.CeilToInt(levelToLoad / (float)maxLevels);
        int selectedIndex = levelToLoad - maxLevels * Mathf.Clamp(round - 1, 0, int.MaxValue);
        return this.levelDefinitionScriptable.GetDefinition(selectedIndex);
    }
}
