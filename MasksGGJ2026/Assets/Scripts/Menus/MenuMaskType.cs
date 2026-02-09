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
            foreach (MaskWin mask in TotemManager.Instance.masks)
            {
                if(mask.mask.Equals(_maskSO))
                {
                    _button.interactable = false;
                    break;
                }
            }
        }
    }
}
