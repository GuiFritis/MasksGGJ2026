using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Utils.Singleton;

public class TotemManager : Singleton<TotemManager>
{
    [SerializeField] private Animator _animator;
    private static readonly int ANIMATION_ID = Animator.StringToHash("TotemShow");
    [SerializeField] private List<SpriteRenderer> _totemParts;
    private List<MaskSO> _masks;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        for (int i = 0; i < _masks.Count; i++)
        {
            _totemParts[i].gameObject.SetActive(true);
            _totemParts[i].transform.GetComponentInChildren<SpriteRenderer>().sprite = _masks[i].sprite;
        }
        _animator.SetTrigger(ANIMATION_ID);
    }
}
