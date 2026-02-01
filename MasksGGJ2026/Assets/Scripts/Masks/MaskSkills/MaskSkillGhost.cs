using System;
using UnityEngine;

public class MaskSkillGhost : MaskSkillBase
{
    [SerializeField] private PlayerBase _playerBase;
    public static Action<bool> OnGhostActive;

    public override void EquipMask(MaskSO mask)
    {
        if(mask != _maskSO) return;
        _playerBase.AllowGhost(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
    }

    private void ChargeSpent(int charges)
    {
        if(charges <= 0)
        {
            _playerBase.AllowGhost(false);
            PlayerMaskManager.onChargeSpent -= ChargeSpent;
        }
    }
}