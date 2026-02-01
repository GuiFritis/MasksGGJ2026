using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerBase))]
public class DeathManager : MonoBehaviour
{

    [SerializeField]
    private UIFade _uiFade;

    void Awake()
    {
        PlayerBase.OnDeath += HandleDeath;
    }
    
    private void HandleDeath()
    {
        _uiFade.FadeOut(() =>
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
}
