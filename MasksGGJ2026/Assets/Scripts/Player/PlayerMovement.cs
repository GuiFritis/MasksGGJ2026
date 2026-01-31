using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigdbody;
    private InGame _inputs;
    private bool _grounded = true;
    public bool IsGrounded => _grounded;

    [SerializeField] private float _playerSpeed = 100f;
    [SerializeField] private float _jumpHeight = 100f;
    private float _direction = 0f;

    [SerializeField] private LayerMask _groundLayer;  

    void OnValidate()
    {
        if(_rigdbody == null)
        {
            _rigdbody = GetComponent<Rigidbody2D>();
        }
    }

    void Awake()
    {            
        _rigdbody = GetComponent<Rigidbody2D>();

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
    private void Move(InputAction.CallbackContext ctx) {
        _direction = ctx.ReadValue<float>();
    }

    private void StopMove(InputAction.CallbackContext ctx) {
        _direction = 0f;
    }

    void MovePlayer() {
        _rigdbody.AddForce(_direction * _playerSpeed * Time.fixedDeltaTime * Vector2.right, ForceMode2D.Force);
    }
    #endregion

    #region JUMP
    private void Jump(InputAction.CallbackContext context) {
        if (_grounded) {
            JumpPlayer();   
        }
    }

    void JumpPlayer() {
        _rigdbody.AddForce(Vector2.up * Time.fixedDeltaTime * _jumpHeight, ForceMode2D.Impulse);

        _grounded = false;
    }
    #endregion

    void FixedUpdate() {
        if (_direction != 0f) {
            MovePlayer();
        }

        if (!_grounded) {
            Vector2 point = new Vector2(Transform.Position.x, Transform.Position.y);
            // Vector2 size = ;
            // float angle = ;

            // Collider2D groundTouched = OverlapBox(point, size, angle, _groundLayer, -Mathf.Infinity, Mathf.Infinity);

            // if (Collider2D) {
            //     _grounded = true;
            // }
        }
    }
}