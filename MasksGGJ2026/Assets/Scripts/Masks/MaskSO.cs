using UnityEngine;

[CreateAssetMenu(fileName = "Mask", menuName = "Scriptable Objects/MaskSO")]
public class MaskSO : ScriptableObject
{
    public string maskName;
    [TextArea] public string description;
    public Sprite sprite;
    public int charges;
    public EMaskSkill eMaskSkill;
    public AnimatorOverrideController animatorOverride;
}

public enum EMaskSkill
{
    DOUBLE_JUMP,
    DASH,
    TELEPORT,
    FREEZE_TIME,
    IMORTALITY
}
