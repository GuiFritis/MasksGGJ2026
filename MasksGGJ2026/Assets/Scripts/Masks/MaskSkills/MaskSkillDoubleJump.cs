using UnityEngine;

public class MaskSkillDoubleJump : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;
    private static PlayerMaskManager _maskManager;

    public override void EquipMask(PlayerMaskManager maskManager)
    {
        _playerMovement.AllowDoubleJump(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
        _maskManager = maskManager;
        PlayerMovement.onDoubleJump += _maskManager.UseSkill;
    }

    private void ChargeSpent(int charges)
    {
        if(charges <= 0)
        {
            _playerMovement.AllowDoubleJump(false);
            PlayerMaskManager.onChargeSpent -= ChargeSpent;
            PlayerMovement.onDoubleJump -= _maskManager.UseSkill;
        }
    }
}
