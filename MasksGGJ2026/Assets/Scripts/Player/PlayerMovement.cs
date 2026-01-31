using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigdbody;
    private InGame _inputs;
    [Header("Movement")]
    [SerializeField] private float _playerSpeed = 750f;
    [SerializeField] public float _groundFriction = 1.5f;
    private float _direction = 0f;
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 200f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _graceTime = 0.05f;
    [SerializeField] private float _jumpLimiter = 0.05f;
    private bool _canDoubleJump = false;
    private bool _doubleJumped = false;
    private float _coyoteTimeCounter;    
    private float _jumpLimiterCounter;
    public static Action onDoubleJump;

    [Header("Ground")]
    [SerializeField] private Vector2 _feetBoxOffset;
    [SerializeField] private Vector2 _feetBoxSize;
    private bool _grounded = true;
    public bool IsGrounded => _grounded;

    void OnValidate()
    {
        if(_rigdbody == null)
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
        _jumpLimiterCounter -= Time.fixedDeltaTime;

        if (_direction != 0f) {
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
    }

    private void StopMove(InputAction.CallbackContext ctx)
    {
        _direction = 0f;
    }

    private void MovePlayer()
    {
        _rigdbody.AddForce(_direction * _playerSpeed * Time.deltaTime * Vector2.right, ForceMode2D.Force);
    }
    #endregion

    #region JUMP
    private void Jump(InputAction.CallbackContext context) 
    {
        if (_coyoteTimeCounter > 0f && _jumpLimiterCounter <= 0f) 
        {
            JumpPlayer();   
        }
        else if(_canDoubleJump && !_doubleJumped)
        {
            DoubleJump();
        }
    }

    private void JumpPlayer() 
    {
        _rigdbody.AddForce(_jumpForce * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);

        _coyoteTimeCounter = 0f;
        _jumpLimiterCounter = _jumpLimiter;

        _grounded = false;
    }

    private void DoubleJump() 
    {
        _doubleJumped = true;
        _rigdbody.linearVelocityY = 0;
        _rigdbody.AddForce(_jumpForce * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);
        onDoubleJump?.Invoke();
    }

    public void AllowDoubleJump(bool canDoubleJump)
    {
        _canDoubleJump = canDoubleJump;
    }
    #endregion

    #region GROUND
    private void CheckGround()
    {
        _coyoteTimeCounter -= Time.fixedDeltaTime;
            
        Collider2D groundTouched = Physics2D.OverlapBox((Vector2)transform.position - _feetBoxOffset, _feetBoxSize, 0, _groundLayer);
    
        if (groundTouched != null) {
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
            if((_groundLayer.value & (1 << collision.gameObject.layer)) != 0 && _direction == 0f)
            {
                if(Mathf.Abs(_rigdbody.linearVelocity.x) > _groundFriction / 5)
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
        if((_groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            _grounded = false;
        }
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube((Vector2)transform.position - _feetBoxOffset, _feetBoxSize);
    }
}
