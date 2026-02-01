using System;
using System.Collections;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private LayerMask _deathLayer;
    public event Action OnDeath;
    private static readonly int DEATH_ID = Animator.StringToHash("Die");
    private static Animator _playerAnimator;
    public static Animator PlayerAnimator => _playerAnimator;

    private bool _canGhost;
    [SerializeField] private float _ghostTime = 3f;
    
    #region COLLISION
    private void OnCollisionEnter2D(Collision2D collision)
    {        
        CheckCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    private void CheckCollision(GameObject gameObject)
    {
        if ((_deathLayer.value & (1 << gameObject.layer)) != 0)
        {
            if (_canGhost) { 
                HandleGhost();
            } else
            {
                Die();
            }
        }
    }
    #endregion

    #region GHOST
    public void AllowGhost(bool allowGhost)
    {
        _canGhost = allowGhost;
    }

    private void HandleGhost() {       
        ActiveGhost();    
        PlayerMaskManager.spendCharge?.Invoke();
    }

    private void ActiveGhost() {
        StartCoroutine(GhostTimer());
        MaskSkillGhost.OnGhostActive?.Invoke(true);
    }

    private IEnumerator GhostTimer()
    {
        yield return new WaitForSeconds(_ghostTime);
        MaskSkillGhost.OnGhostActive?.Invoke(false);
    }
    #endregion

    #region DEATH
    private void Die()
    {
        Debug.Log("Player is dead");
        _playerAnimator.SetTrigger(DEATH_ID);
        OnDeath?.Invoke();
    }
    #endregion
}