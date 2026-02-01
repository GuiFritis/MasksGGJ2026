using System;
using System.Collections;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private LayerMask _deathLayer;
    public static Action OnDeath;
    private static readonly int DEATH_ID = Animator.StringToHash("Die");
    [SerializeField] private Animator _animator;
    private static Animator _playerAnimator;
    public static Animator PlayerAnimator => _playerAnimator;
    [SerializeField] private AudioSO _deathAudio;
    [SerializeField] private AudioSO _resurrectionAudio;

    private bool _canGhost;
    [SerializeField] private float _ghostTime = 3f;

    void Awake()
    {
        _playerAnimator = _animator;
        _playerAnimator.runtimeAnimatorController = GameManager.Instance.SelectedMask.animatorOverride;
    }

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
        SFX_Pool.Instance.Play(_resurrectionAudio);
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
        SFX_Pool.Instance.Play(_deathAudio);
        _playerAnimator.SetTrigger(DEATH_ID);
        OnDeath?.Invoke();
    }
    #endregion
}