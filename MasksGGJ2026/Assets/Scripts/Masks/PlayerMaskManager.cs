using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaskManager : MonoBehaviour
{
    [SerializeField] private List<MaskSkillBase> _maskSkills;
    private int _currentCharges = 0;
    public static Action<MaskSO> onMaskEquiped;
    public static Action<int> onChargeSpent;
    public static Action spendCharge;

    void Awake()
    {
        SetMask(_maskSkills[0].GetMaskSO);
        spendCharge += SpendCharge;
    }

    public void SetMask(MaskSO mask)
    {
        _currentCharges = mask.charges;
        onMaskEquiped?.Invoke(mask);       
    }

    public void SpendCharge()
    {
        _currentCharges--;
        onChargeSpent?.Invoke(_currentCharges);
    }
}