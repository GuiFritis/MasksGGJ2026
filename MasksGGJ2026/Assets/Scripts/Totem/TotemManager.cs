using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class TotemManager : Singleton<TotemManager>
{
    [SerializeField] private Animator _animator;
    private static readonly int ANIMATION_ID = Animator.StringToHash("TotemShow");
    [SerializeField] private List<TotemPart> _totemParts = new();
    private List<MaskSO> _usedMasks = new();
    public bool HasUsedMask (MaskSO mask) => _usedMasks.Contains(mask);

    [SerializeField] UIFade _uiFade;

    protected override void Awake()
    {
        if(Instance == null){
            Instance = GetComponent<TotemManager>();
        } else {
            Instance.Enter(transform.position);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Enter(transform.position);
    }

    public void Enter(Vector3 position)
    {
        transform.position = position;
        _uiFade.FadeIn(() => _animator.SetTrigger(ANIMATION_ID));  
    }



    public void AddMask(MaskSO mask, bool win)
    {
        _usedMasks.Add(mask);
        if(win)
        {
            TotemPart part = _totemParts.Find(i => i.mask.Equals(mask));
            part.Win();
            part.maskSprite.sprite = mask.sprite;
        }
        _animator.SetTrigger(ANIMATION_ID);
    }
}

[System.Serializable]
public class TotemPart
{
    public bool win;
    public MaskSO mask;
    public SpriteRenderer maskSprite;

    public void Win()
    {
        win = true;
    }
}
