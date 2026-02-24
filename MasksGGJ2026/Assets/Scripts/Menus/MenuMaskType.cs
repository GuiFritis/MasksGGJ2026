using UnityEngine;
using UnityEngine.UI;

public class MenuMaskType : MonoBehaviour
{
    [SerializeField] private MaskSO _maskSO;
    public MaskSO MaskSO => _maskSO;
    [SerializeField] private Button _button;

    void Awake()
    {
        if(TotemManager.Instance != null)
        {
            if(TotemManager.Instance.HasUsedMask(_maskSO))
            {
                _button.interactable = false;
            }
        }
    }
}
