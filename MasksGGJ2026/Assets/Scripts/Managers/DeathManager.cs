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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
