using System;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private LayerMask _deathLayer;
    public event Action OnDeath;
    private static readonly int DEATH_ID = Animator.StringToHash("Die");
    private static Animator _playerAnimator;
    public static Animator PlayerAnimator => _playerAnimator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((_deathLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_deathLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead");
        _playerAnimator.SetTrigger(DEATH_ID);
        OnDeath?.Invoke();
    }
}
