using System;
using UnityEngine;

public class MaskSkillGhost : MaskSkillBase
{
    [SerializeField] private PlayerMovement _playerMovement;
    private static PlayerMaskManager _maskManager; 
    public static Action<bool> OnGhostActive;
    private float _ghostTimer = 3f;
    private bool _isGhost = false;

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

    private void Update() {
        if (_isGhost) {
            if (_ghostTimer > 0) {
                _ghostTimer -= Time.deltaTime;
            } else {
                ActiveGhost(true);
            }
        }
    }

    public void ActiveGhost(bool isGhost) {
        OnGhostActive?.Invoke(isGhost);
    }
}
