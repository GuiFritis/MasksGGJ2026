using UnityEngine;

public class MaskSkillTeleport : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;
    private static PlayerMaskManager _maskManager; 

    public override void EquipMask(PlayerMaskManager maskManager)
    {
        _playerMovement.AllowTeleport(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
        _maskManager = maskManager;
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
