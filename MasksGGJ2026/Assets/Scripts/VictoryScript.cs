using System;
using UnityEngine;

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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TotemManager.Instance.AddMask(_mask, true);
        }
    }
}
