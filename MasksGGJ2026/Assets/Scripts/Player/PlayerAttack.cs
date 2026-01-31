using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D _rigdbody;
    private PlayerMovement _playerMovement;
    private InGame _inputs;

    void OnValidate()
    {
        if(_rigdbody == null)
        {
            _rigdbody = GetComponent<Rigidbody2D>();
        }
        if(_playerMovement == null)
        {
            _playerMovement = GetComponent<PlayerMovement>();
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
        _inputs.Gameplay.Attack.started += Attack;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if(_playerMovement.IsGrounded)
        {
            //? Aqui deve ser realizado o ataque
        }
    }
}