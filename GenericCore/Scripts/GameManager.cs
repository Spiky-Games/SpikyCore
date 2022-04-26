using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private LevelDefinitionScriptable levelDefinitionScriptable;

    public static GameManager Instance
    {
        get;
        set;
    }

    public void Start()
    {
        Instance = this;
        this.StartLevel();
    }

    private void StartLevel()
    {
        LevelDefinition levelDefinition = DefaultLevelManager.Instance.LoadLevel(DefaultUserManager.Instance.UserData.CurrentLevel);
        SceneManager.LoadScene(levelDefinition.sceneToLoad.name);
    }
}
