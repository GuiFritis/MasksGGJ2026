using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MaskSO SelectedMask;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
