using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

[RequireComponent(typeof(Collider2D))]
public class VictoryScript : MonoBehaviour
{
    private MaskSO _mask;

    void Awake()
    {
        PlayerMaskManager.onMaskEquiped += EquipMask;
        PlayerBase.OnDeath += GameOver;
    }

    private void EquipMask(MaskSO mask)
    {
        _mask = mask;
    }

    private void GameOver()
    {
        TotemManager.Instance.AddMask(_mask, false);
        Invoke(nameof(BackToMaskSelection), .5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TotemManager.Instance.AddMask(_mask, true);
            Invoke(nameof(BackToMaskSelection), .5f);
        }
    }

    private void BackToMaskSelection()
    {
        SceneManager.LoadScene(1);
    }
}
