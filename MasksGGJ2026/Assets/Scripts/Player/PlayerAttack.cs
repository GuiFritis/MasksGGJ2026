using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    private InGame _inputs;
    private static readonly int ATTACK_ID = Animator.StringToHash("Attack");
    [SerializeField] private Vector2 _attackOffset;
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask _pogoLayer;
    [SerializeField] private float _knockback;
    [SerializeField] private float _attackCooldown;
    [Header("Audio")]
    [SerializeField] private AudioSO _attackAudio;
    [SerializeField] private AudioSO _hitAudio;

    private float _attackTimer = 0f;

    private bool CanAttack => _attackTimer <= 0f;

    void OnValidate()
    {
        if (_playerMovement == null)
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
    }

    void Awake()
    {
        SetUpInputs();
    }

    void Update()
    {
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }
    }

    private void SetUpInputs()
    {
        _inputs = new();
        _inputs.Enable();
        _inputs.Gameplay.Attack.started += Attack;
    }

    void OnDisable()
    {
        _inputs.Disable();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (CanAttack)
        {
            SFX_Pool.Instance.Play(_attackAudio);
            PlayerBase.PlayerAnimator.SetTrigger(ATTACK_ID);
            _attackTimer = _attackCooldown;

            Vector2 attackPosition = (Vector2)transform.position + _attackOffset;

            Collider2D[] hits = Physics2D.OverlapCircleAll(
                attackPosition,
                _attackRadius,
                _pogoLayer
            );

            if (hits.Length == 0)
                return;

            
            SFX_Pool.Instance.Play(_hitAudio);
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<Dodo>(out Dodo dodo))
                {
                    dodo.HandlePogo();
                }
            }

            _playerMovement.PlayerRigdbody.linearVelocityY = 0;

            _playerMovement.PlayerRigdbody.AddForce(Vector2.up * _knockback, ForceMode2D.Impulse);   
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackOffset + (Vector2)transform.position, _attackRadius);
    }
}