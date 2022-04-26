public interface ILevelManager
{
    public LevelDefinition LoadLevel(int levelIndex);
    public void CompleteLevel();
}
