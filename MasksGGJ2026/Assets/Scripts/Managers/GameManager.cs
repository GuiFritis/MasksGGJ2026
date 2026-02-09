using UnityEngine;
using Utils.Singleton;

public class GameManager : Singleton<GameManager>
{
    public MaskSO SelectedMask;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
