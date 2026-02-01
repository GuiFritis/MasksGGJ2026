using System;
using UnityEngine;

public class MaskSkillFreezeTime : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;
    private static PlayerMaskManager _maskManager; 
    public static Action<bool> OnFreezeTime;

    public override void EquipMask(PlayerMaskManager maskManager)
    {
        _playerMovement.AllowFreezeTime(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
        _maskManager = maskManager;
    }

    private void ChargeSpent(int charges)
    {
        if(charges <= 0)
        {
            _playerMovement.AllowFreezeTime(false);
            PlayerMaskManager.onChargeSpent -= ChargeSpent;
        }
    }
}