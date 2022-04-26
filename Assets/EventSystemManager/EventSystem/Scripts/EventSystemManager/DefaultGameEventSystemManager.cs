using EventSystem;
using UnityEngine;

public class DefaultGameEventSystemManager : MonoBehaviour, IGameEventSystemManager
{
    public GameEventSystem GameEventSystem
    {
        get;
    } = new GameEventSystem();


    public static DefaultGameEventSystemManager Instance
    {
        get;
        set;
    }

    public void Awake()
    {
        Instance = this;
    }
}
