using System;
using UnityEngine;
using UnityEngine.UI;

public class MaskChargesUI : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _widthPerUnity;

    void Awake()
    {
        PlayerMaskManager.onMaskEquiped += EquipeMask;
        PlayerMaskManager.onChargeSpent += ChargeSpent;
    }

    void OnDisable()
    {
        PlayerMaskManager.onMaskEquiped -= EquipeMask;
        PlayerMaskManager.onChargeSpent -= ChargeSpent;
    }

    private void EquipeMask(MaskSO maskSO)
    {
        _rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal, 
            _widthPerUnity * maskSO.charges
        );
    }

    private void ChargeSpent(int charges)
    {
        _rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal, 
            _widthPerUnity * charges
        );
    }

}
