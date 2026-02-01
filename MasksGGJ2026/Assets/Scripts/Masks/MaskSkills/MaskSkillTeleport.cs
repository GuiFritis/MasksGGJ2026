using UnityEngine;

public class MaskSkillTeleport : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;

    public override void EquipMask(MaskSO mask)
    {
        if(mask != _maskSO) return;
        _playerMovement.AllowTeleport(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
    }

    private void ChargeSpent(int charges)
    {
        if(charges <= 0)
        {
            _playerMovement.AllowTeleport(false);
            PlayerMaskManager.onChargeSpent -= ChargeSpent;
        }
    }
}
