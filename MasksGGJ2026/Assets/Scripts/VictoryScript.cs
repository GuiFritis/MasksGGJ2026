using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class VictoryScript : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private MaskSO _mask;

    void Awake()
    {
        PlayerMaskManager.onMaskEquiped += EquipMask;
        PlayerBase.OnDeath += GameOver;
    }

    void OnDisable()
    {
        PlayerMaskManager.onMaskEquiped -= EquipMask;
        PlayerBase.OnDeath -= GameOver;
    }

    private void EquipMask(MaskSO mask)
    {
        _mask = mask;
    }

    private void GameOver()
    {
        TotemManager.Instance.AddMask(_mask, false);
        TotemManager.Instance.Enter(_playerTransform.position);
        Invoke(nameof(BackToMaskSelection), 3f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TotemManager.Instance.AddMask(_mask, true);
            TotemManager.Instance.Enter(_playerTransform.position);
            Invoke(nameof(BackToMaskSelection), 3f);
        }
    }

    private void BackToMaskSelection()
    {
        SceneManager.LoadScene(1);
    }
}
