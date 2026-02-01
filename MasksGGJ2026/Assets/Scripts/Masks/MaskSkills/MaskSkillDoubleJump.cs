using UnityEngine;

public class MaskSkillDoubleJump : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;

    public override void EquipMask(MaskSO mask)
    {
        if(mask != _maskSO) return;
        _playerMovement.AllowDoubleJump(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
    }

    private void ChargeSpent(int charges)
    {
        if(charges <= 0)
        {
            _playerMovement.AllowDoubleJump(false);
            PlayerMaskManager.onChargeSpent -= ChargeSpent;
        }
    }
}
