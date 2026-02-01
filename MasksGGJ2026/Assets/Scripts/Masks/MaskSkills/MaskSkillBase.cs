using UnityEngine;

public abstract class MaskSkillBase : MonoBehaviour
{    
    [SerializeField] protected MaskSO _maskSO;
    public MaskSO GetMaskSO => _maskSO;

    void Awake()
    {
        PlayerMaskManager.onMaskEquiped += EquipMask;
    }

    public abstract void EquipMask(MaskSO mask);
}
