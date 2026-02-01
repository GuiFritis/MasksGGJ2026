using System;
using UnityEngine;

public class MaskSkillGhost : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;
    private static PlayerMaskManager _maskManager; 
    public static Action<bool> onGhostActive;

    public override void EquipMask(PlayerMaskManager maskManager)
    {
        PlayerMaskManager.onChargeSpent += ChargeSpent;
        _maskManager = maskManager;
    }

    private void ChargeSpent(int charges)
    {
        if(charges <= 0)
        {
            PlayerMaskManager.onChargeSpent -= ChargeSpent;
        }
    }

    private void HandleGhost(bool isGhost) 
    {
        onGhostActive?.Invoke(isGhost);
    }
}
