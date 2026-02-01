using System;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private LayerMask _deathLayer;

    public event Action OnDeath;

    private bool _isGhost = false;
    private bool _isFirstHit = true;
    [SerializeField] private bool _isUsingGhostMask = false;

    private void OnEnable()
    {
        MaskSkillGhost.OnGhostActive += AlternateGhost;
    }

    private void OnDisable()
    {
        MaskSkillGhost.OnGhostActive -= AlternateGhost;
    }

    private void AlternateGhost(bool isGhost) {
        _isGhost = isGhost;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (_isUsingGhostMask) {
            
        Debug.Log("está de mascara!");
        HandleGhost();
        }
        
        if ((_deathLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Die();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isUsingGhostMask) {
            HandleGhost();
        }

        if ((_deathLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Die();
        }
    }

    private void HandleGhost() {
        if (_isGhost) {
        Debug.Log("fantasma");
            return;
        }

        if (_isFirstHit) {
        Debug.Log("first hit");
            MaskSkillGhost.OnGhostActive(true);
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead");
        OnDeath?.Invoke();
    }
}
