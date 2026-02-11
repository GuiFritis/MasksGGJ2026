using System.Collections;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private AudioSource _audioSource;
    private LayerMask _startingLayer;

    void Awake()
    {
        _startingLayer = gameObject.layer;
    }

    void OnEnable()
    {
        MaskSkillFreezeTime.OnFreezeTime += Freeze;
    }

    void OnDisable()
    {
        MaskSkillFreezeTime.OnFreezeTime -= Freeze;
    }

    private void Freeze(bool freeze)
    {
        StartCoroutine(Freezing(freeze ? 0 : 1, freeze ? 1 : 0));
        if(freeze)
        {
            gameObject.layer = 0;
            _collider.isTrigger = false;
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
        _collider.isTrigger = targetVal == 1;
        if(targetVal == 1)
        {
            gameObject.layer = _startingLayer;
        }
    }
}
