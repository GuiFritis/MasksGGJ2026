using UnityEngine;

public abstract class MaskSkillBase : MonoBehaviour
{    
    [SerializeField] protected MaskSO _maskSO;
    public MaskSO GetMaskSO => _maskSO;

    public abstract void EquipMask(PlayerMaskManager maskManager);
}
