using System;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private LayerMask _deathLayer;

    public event Action OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if ((_deathLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log("Player is dead");
        OnDeath?.Invoke();
    }
}
