using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerBase))]
public class DeathManager : MonoBehaviour
{
    private PlayerBase _player;

    [SerializeField]
    private UIFade _uiFade;

    void Awake()
    {
        _player = GetComponent<PlayerBase>();
        _player.OnDeath += HandleDeath;
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
