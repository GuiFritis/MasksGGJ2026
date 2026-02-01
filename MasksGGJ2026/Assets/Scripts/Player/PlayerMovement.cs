using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[DefaultExecutionOrder(-1)]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigdbody;
    private static readonly int WALK_ID = Animator.StringToHash("Velocity");
    private static readonly int JUMP_ID = Animator.StringToHash("Jump");
    private static readonly int FALL_ID = Animator.StringToHash("Falling");
    private static readonly int DASH_ID = Animator.StringToHash("Dash");
    private InGame _inputs;
    [Header("Movement")]
    [SerializeField] private float _playerSpeed = 750f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] public float _groundFriction = 1.5f;
    private float _direction = 0f;

    [Header("Dash")]
    [SerializeField] private float _dashSpeed = 3000f;
    private bool _isDashing;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 200f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _graceTime = 0.05f;
    [SerializeField] private float _jumpLimiter = 0.05f;
    private bool _canDoubleJump = false;
    private bool _doubleJumped = false;
    private float _coyoteTimeCounter;
    private float _jumpLimiterCounter;

    [Header("Ground")]
    [SerializeField] private Vector2 _feetBoxOffset;
    [SerializeField] private Vector2 _feetBoxSize;
    private bool _grounded = true;
    public bool IsGrounded => _grounded;
    
    [Header("Freeze Time")]    
    [SerializeField] private float _freezeTime = 3f;

    [Header("Throw action")]
    [SerializeField]
    private GameObject _throwablePrefab;
    [SerializeField]
    private Transform _throwPoint;
    [SerializeField]
    private float _throwForce = 10f;

    private bool _facingRight = true;
    private GameObject _activeProjectile;

    void OnValidate()
    {
        if (_rigdbody == null)
        {
            _rigdbody = GetComponent<Rigidbody2D>();
        }
    }

    void Awake()
    {
        SetUpInputs();
    }

    private void SetUpInputs()
    {
        _inputs = new();
        _inputs.Enable();
        _inputs.Gameplay.Move.performed += Move;
        _inputs.Gameplay.Move.canceled += StopMove;
        _inputs.Gameplay.Jump.started += Jump;
    }

    void FixedUpdate()
    {
        PlayerBase.PlayerAnimator.SetFloat(WALK_ID, MathF.Min(_rigdbody.linearVelocityX, 1f));
        _jumpLimiterCounter -= Time.fixedDeltaTime;

        if (_direction != 0f && !_isDashing)
        {
            MovePlayer();
        }

        if (_grounded)
        {
            _coyoteTimeCounter = _graceTime;
            _jumpLimiterCounter = 0f;
        }
        else
        {
            CheckGround();
        }
    }

    #region MOVEMENT
    private void Move(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<float>();
        if (_facingRight && _direction < 0)
        {
            Flip();
        }

        if (!_facingRight && _direction > 0)
        {
            Flip();
        }
    }
    public void Flip()
    {
        _facingRight = !_facingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void StopMove(InputAction.CallbackContext ctx)
    {
        _direction = 0f;
    }

    private void MovePlayer()
    {
        _rigdbody.AddForce(_direction * _playerSpeed * Time.deltaTime * Vector2.right, ForceMode2D.Force);
        
        if(Mathf.Abs(_rigdbody.linearVelocityX) > _maxSpeed)
        {
            _rigdbody.linearVelocityX = _maxSpeed * Mathf.Sign(_rigdbody.linearVelocityX);
        }
    }
    #endregion

    #region DASH
    public void AllowDash(bool allow)
    {
        if (allow)
        {
            _inputs.Gameplay.UseMask.performed += Dash;
        }
        else
        {
            _inputs.Gameplay.UseMask.performed -= Dash;
        }
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (_direction != 0 && !_isDashing)
        {
            PlayerBase.PlayerAnimator.SetTrigger(DASH_ID);
            _isDashing = true;
            PlayerMaskManager.spendCharge?.Invoke();
            _rigdbody.gravityScale = 0;
            _rigdbody.linearVelocityY = 0;
            _rigdbody.AddForce(_direction * _dashSpeed * Vector2.right, ForceMode2D.Impulse);
            StartCoroutine(DashReturnGravity());
        }
    }

    private IEnumerator DashReturnGravity()
    {
        yield return new WaitForSeconds(0.3f);
        _rigdbody.gravityScale = 1;
        _rigdbody.linearVelocityX = 0;
        _isDashing = false;
        MovePlayer();
    }
    #endregion

    #region JUMP
    private void Jump(InputAction.CallbackContext context)
    {
        if (_coyoteTimeCounter > 0f && _jumpLimiterCounter <= 0f)
        {
            JumpPlayer();
        }
        else if (_canDoubleJump && !_doubleJumped)
        {
            DoubleJump();
        }
    }

    private void JumpPlayer()
    {
        PlayerBase.PlayerAnimator.SetTrigger(JUMP_ID);
        PlayerBase.PlayerAnimator.SetBool(FALL_ID, true);
        _rigdbody.AddForce(_jumpForce * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);

        _coyoteTimeCounter = 0f;
        _jumpLimiterCounter = _jumpLimiter;

        _grounded = false;
    }

    private void DoubleJump()
    {
        PlayerBase.PlayerAnimator.SetTrigger(JUMP_ID);
        _doubleJumped = true;
        _rigdbody.linearVelocityY = 0;
        _rigdbody.AddForce(_jumpForce * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);
        PlayerMaskManager.spendCharge?.Invoke();
    }

    public void AllowDoubleJump(bool canDoubleJump)
    {
        _canDoubleJump = canDoubleJump;
    }
    #endregion

    #region TELEPORT
    public void AllowTeleport(bool allow)
    {
        if (allow)
        {
            _inputs.Gameplay.UseMask.performed += TryTeleport;
        }
        else
        {
            _inputs.Gameplay.UseMask.performed -= TryTeleport;
        }
    }

    void TryTeleport(InputAction.CallbackContext context)
    {
        if (_activeProjectile != null)
            return;

        GameObject projectile = Instantiate(
            _throwablePrefab,
            _throwPoint.position,
            Quaternion.identity
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        Vector2 direction = _facingRight ? Vector2.right : Vector2.left;
        rb.linearVelocityX = direction.x * _throwForce;

        _activeProjectile = projectile;

        projectile
            .GetComponent<MaskTeleportProjectile>()
            .SetOwner(this);

        PlayerMaskManager.spendCharge?.Invoke();

    }

    public void ClearProjectile()
    {
        _activeProjectile = null;
    }
    #endregion

    #region GROUND
    private void CheckGround()
    {
        _coyoteTimeCounter -= Time.fixedDeltaTime;

        Collider2D groundTouched = Physics2D.OverlapBox((Vector2)transform.position - _feetBoxOffset, _feetBoxSize, 0, _groundLayer);
    
        if (groundTouched != null) {
            PlayerBase.PlayerAnimator.SetBool(FALL_ID, false);
            _grounded = true;
            _doubleJumped = false;
        }
    }

    private void GroundedFriction()
    {
        float _accelerationX = _groundFriction;

        _accelerationX *= Mathf.Sign(_rigdbody.linearVelocity.x) * -1;

        _rigdbody.AddForce(Vector2.right * _accelerationX, ForceMode2D.Force);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if ((_groundLayer.value & (1 << collision.gameObject.layer)) != 0 && _direction == 0f)
        {
            if (Mathf.Abs(_rigdbody.linearVelocity.x) > _groundFriction / 5)
            {
                GroundedFriction();
            }
            else
            {
                _rigdbody.linearVelocity *= Vector2.up;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if ((_groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            _grounded = false;
            PlayerBase.PlayerAnimator.SetBool(FALL_ID, true);
        }
    }
    #endregion

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawWireCube((Vector2)transform.position - _feetBoxOffset, _feetBoxSize);
    // }
    
    #region FREEZE TIME
    public void AllowFreezeTime(bool allow)
    {
        if(allow)
        {
            _inputs.Gameplay.UseMask.performed += FreezeTime;
        }
        else
        {
            _inputs.Gameplay.UseMask.performed -= FreezeTime;            
        }
    }

    private void FreezeTime(InputAction.CallbackContext context) {
        StartCoroutine(FreezeTimer());
        MaskSkillFreezeTime.OnFreezeTime?.Invoke(true);
        PlayerMaskManager.spendCharge?.Invoke();
    }

    private IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(_freezeTime);
        MaskSkillFreezeTime.OnFreezeTime?.Invoke(false);
    }
    #endregion
}