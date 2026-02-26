using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Saw : MonoBehaviour, ITranslocatable
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _damageCollider;
    [SerializeField] private Collider2D _platformCollider;
    [SerializeField] private AudioSource _audioSource;
    private LayerMask _startingLayer;

    void Awake()
    {
        _startingLayer = gameObject.layer;
    }

    void OnEnable()
    {
        MaskSkillFreezeTime.OnFreezeTime += Freeze;
        MaskSkillGhost.OnGhostActive += GhostMode;
    }

    void OnDisable()
    {
        MaskSkillFreezeTime.OnFreezeTime -= Freeze;
        MaskSkillGhost.OnGhostActive -= GhostMode;
    }

    private void Freeze(bool freeze)
    {
        StartCoroutine(Freezing(freeze ? 0 : 1, freeze ? 1 : 0));
        if(freeze)
        {
            _damageCollider.enabled = false;
            _platformCollider.enabled = true;
        }
    }

    IEnumerator Freezing(float targetVal, float startValue)
    {
        float timer = 0;
        while(timer < .4f)
        {
            float lerpedValue = Mathf.Lerp(startValue, targetVal, timer/.5f);
            _animator.speed = lerpedValue;
            _audioSource.volume = lerpedValue;
            timer += Time.deltaTime;
            yield return null;
        }        
        _animator.speed = targetVal;
        _audioSource.volume = targetVal;
        _damageCollider.enabled = targetVal == 1;
        _platformCollider.enabled = targetVal != 1;
    }

    public Vector3 TranslocatePosition()
    {
        return transform.position;
    }

    public void SwitchPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void GhostMode(bool active)
    {
        _sprite.color = active ? MaskSkillGhost.TRANSLUCID_WHITE : Color.white;
        _damageCollider.enabled = !active;
    }
}
