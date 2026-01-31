using UnityEngine;

public class MaskSkillDash : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;
    private static PlayerMaskManager _maskManager; 

    public override void EquipMask(PlayerMaskManager maskManager)
    {
        _playerMovement.AllowDash(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
        _maskManager = maskManager;
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
