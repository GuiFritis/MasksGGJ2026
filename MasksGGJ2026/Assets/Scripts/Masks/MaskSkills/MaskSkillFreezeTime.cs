using System;
using UnityEngine;

public class MaskSkillFreezeTime : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;
    public static Action<bool> OnFreezeTime;

    public override void EquipMask(MaskSO mask)
    {
        if(mask != _maskSO) return;
        _playerMovement.AllowFreezeTime(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
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