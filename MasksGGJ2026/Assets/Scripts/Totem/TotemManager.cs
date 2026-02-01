using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class TotemManager : Singleton<TotemManager>
{
    [SerializeField] private Animator _animator;
    private static readonly int ANIMATION_ID = Animator.StringToHash("TotemShow");
    [SerializeField] private List<SpriteRenderer> _totemParts;
    public List<MaskWin> masks = new();
    [SerializeField] UIFade _uiFade;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < masks.Count; i++)
        {
            if(masks[i].win)
            {                
                _totemParts[i].gameObject.SetActive(true);
                _totemParts[i].transform.GetComponentInChildren<SpriteRenderer>().sprite = masks[i].mask.sprite;
            }
        }
        _uiFade.FadeIn(() => _animator.SetTrigger(ANIMATION_ID));        
    }

    public void AddMask(MaskSO mask, bool win)
    {
        masks.Add(new()
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
