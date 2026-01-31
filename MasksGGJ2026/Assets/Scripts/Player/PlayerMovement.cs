using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigdbody;
    private InGame _inputs;
    private bool _grounded = true;
    public bool IsGrounded => _grounded;
    

    [Header("Movement")]
    [SerializeField] private float _playerSpeed = 750f;
    [SerializeField] private float _jumpHeight = 200f;
    private float _direction = 0f;

    [SerializeField] private Renderer _playerRenderer;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private Vector2 _feetBoxOffset;
    [SerializeField] private Vector2 _feetBoxSize;

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
    private void Jump(InputAction.CallbackContext context) {
        if (_grounded) {
            JumpPlayer();   
        }
    }

    void JumpPlayer() {
        _rigdbody.AddForce(_jumpHeight * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);

        _grounded = false;
    }
    #endregion

    void FixedUpdate() {
        if (_direction != 0f) {
            MovePlayer();
        }

        if (!_grounded) {
            Collider2D groundTouched = Physics2D.OverlapBox((Vector2)transform.position - _feetBoxOffset, _feetBoxSize, 0, _groundLayer);
            if (groundTouched != null) {
                _grounded = true;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube((Vector2)transform.position - _feetBoxOffset, _feetBoxSize);
    }
}
