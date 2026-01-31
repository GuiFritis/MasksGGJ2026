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
    [SerializeField] private float _playerSpeed = 2f;
    private float _direction = 0f;

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
        _inputs.Gameplay.Jump.started += Jump;
    }

    private void FixedUpdate()
    {
        if(_direction != 0)
        {
            //movimenta
        }
    }

    #region MOVEMENT
    private void Move(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<float>();
        _rigdbody.AddForce(_direction * _playerSpeed * Time.deltaTime * Vector2.right);
    }
    #endregion

    #region JUMP
    private void Jump(InputAction.CallbackContext context)
    {
        //? Fazer aqui a configuração do pulo do personagem
    }
    #endregion
}
