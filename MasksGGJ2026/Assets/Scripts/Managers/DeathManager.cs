using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerBase))]
public class DeathManager : MonoBehaviour
{
    private PlayerBase _player;

    [SerializeField]
    private UIFadeOut _uiFadeOut;

    void Awake()
    {
        _player = GetComponent<PlayerBase>();
        _player.OnDeath += HandleDeath;
    }
    
    private void HandleDeath()
    {
        _uiFadeOut.FadeOut(() =>
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
}
