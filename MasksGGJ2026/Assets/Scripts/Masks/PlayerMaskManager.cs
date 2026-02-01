using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaskManager : MonoBehaviour
{
    [SerializeField] private List<MaskSkillBase> _maskSkills;
    private MaskSO _mask;
    private int _currentCharges = 0;
    public static Action<int> onChargeSpent;
    public static Action spendCharge;

    void Awake()
    {
        SetMask(_maskSkills[0].GetMaskSO);
        spendCharge += SpendCharge;
    }

    public void SetMask(MaskSO mask)
    {
        _mask = mask;
        _currentCharges = _mask.charges;
        _maskSkills.Find(i => i.GetMaskSO.Equals(_mask))?.EquipMask(this);        
    }

    public void SpendCharge()
    {
        _currentCharges--;
        onChargeSpent?.Invoke(_currentCharges);
    }
}