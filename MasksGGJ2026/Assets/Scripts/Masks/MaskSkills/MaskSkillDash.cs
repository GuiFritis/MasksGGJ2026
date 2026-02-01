using UnityEngine;

public class MaskSkillDash : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;

    public override void EquipMask(MaskSO mask)
    {
        if(mask != _maskSO) return;
        _playerMovement.AllowDash(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
    }

    private void ChargeSpent(int charges)
    {
        if(charges <= 0)
        {
            _playerMovement.AllowDash(false);
            PlayerMaskManager.onChargeSpent -= ChargeSpent;
        }
    }
}
