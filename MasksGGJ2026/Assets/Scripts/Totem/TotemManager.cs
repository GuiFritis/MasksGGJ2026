using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class TotemManager : Singleton<TotemManager>
{
    [SerializeField] private Animator _animator;
    private static readonly int ANIMATION_ID = Animator.StringToHash("TotemShow");
    [SerializeField] private List<SpriteRenderer> _totemParts;
    private List<MaskWin> _masks = new();
    [SerializeField] UIFadeOut _uiFadeOut;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < _masks.Count; i++)
        {
            if(_masks[i].win)
            {                
                _totemParts[i].gameObject.SetActive(true);
                _totemParts[i].transform.GetComponentInChildren<SpriteRenderer>().sprite = _masks[i].mask.sprite;
            }
        }
        _uiFadeOut.FadeIn(() => _animator.SetTrigger(ANIMATION_ID));        
    }

    public void AddMask(MaskSO mask, bool win)
    {
        _masks.Add(new()
        {
            mask = mask,
            win = win
        });
    }
}

public struct MaskWin
{
    public MaskSO mask;
    public bool win;
}
