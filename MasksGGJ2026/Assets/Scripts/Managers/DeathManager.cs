using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerBase))]
public class DeathManager : MonoBehaviour
{

    [SerializeField]
    private UIFadeOut _uiFadeOut;

    void Awake()
    {
        PlayerBase.OnDeath += HandleDeath;
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
