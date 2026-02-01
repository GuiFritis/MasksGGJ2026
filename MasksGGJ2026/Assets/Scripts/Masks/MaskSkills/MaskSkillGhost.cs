using System;
using System.Collections;
using UnityEngine;

public class MaskSkillGhost : MaskSkillBase
{
    [SerializeField] private PlayerBase _playerBase;
    private static PlayerMaskManager _maskManager; 
    public static Action<bool> OnGhostActive;

    public override void EquipMask(PlayerMaskManager maskManager)
    {
        _playerBase.AllowGhost(true);
        PlayerMaskManager.onChargeSpent += ChargeSpent;
        _maskManager = maskManager;
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