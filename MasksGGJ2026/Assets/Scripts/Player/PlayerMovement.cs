using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigdbody;
    private InGame _inputs;
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
        _inputs.Gameplay.Jump.started += Jump;
    }

    #region MOVEMENT
    private void Move(InputAction.CallbackContext ctx)
    {
        float direction = ctx.ReadValue<float>();
        //Aqui o personagem deve ser movido de acordo com a direção
    }
    #endregion

    #region JUMP
    private void Jump(InputAction.CallbackContext context)
    {
        //? Fazer aqui a configuração do pulo do personagem
    }
    #endregion
}
