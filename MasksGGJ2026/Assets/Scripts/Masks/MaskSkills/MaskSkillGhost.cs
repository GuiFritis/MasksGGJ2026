using System;
using UnityEngine;

public class MaskSkillGhost : MaskSkillBase
{
    [SerializeField] private PlayerBase _playerBase;
    public static Action<bool> OnGhostActive;
    public static readonly Color TRANSLUCID_WHITE = new(1, 1, 1, .3f);

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
            Invoke(nameof(DisableGhost), .6f);
            PlayerMaskManager.onChargeSpent -= ChargeSpent;
        }
    }

    private void DisableGhost()
    {
        _playerBase.AllowGhost(false);
    }
}