using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceTrap : MonoBehaviour
{
    [SerializeField] private GameObject _lanceObject;
    [SerializeField] private List<Collider2D> _colliders = new();
    [Header("Timing")]
    [SerializeField] private bool _isOdd;
    [SerializeField] private float _delay = 1.5f;
    [SerializeField] private float _riseTime = 0.5f;
    private float _elapsed;
    private bool _isTimeFrozen;

    [Header("Rise")]
    [SerializeField] private float _startOffset = 1f;
    [SerializeField] private float _finalOffset = 1f;
    
    private void OnEnable()
    {
        MaskSkillFreezeTime.OnFreezeTime += AlternateTime;
    }

    private void OnDisable()
    {
        MaskSkillFreezeTime.OnFreezeTime -= AlternateTime;
    }

    void Start()
    {
        StartCoroutine(LanceRoutine());
    }

    IEnumerator LanceRoutine()
    {
        if(_isOdd)
        {
            yield return new WaitForSeconds(_delay + _riseTime);
        }
        while (true)
        {    

            SwitchColliders(true);

            yield return Rise(
                transform.position + transform.up * _startOffset, 
                transform.position + transform.up * _finalOffset
            );

            yield return new WaitForSeconds(_delay);

            yield return Rise(
                transform.position + transform.up * _finalOffset,
                transform.position + transform.up * _startOffset
            );

            SwitchColliders(false);

            yield return new WaitForSeconds(_delay);
        }
    }

    IEnumerator Rise(Vector3 startPosition, Vector3 finalPosition)
    {
        _elapsed = 0f;

        while (_elapsed < _riseTime)
        {
            yield return new WaitWhile(() => _isTimeFrozen);

            _lanceObject.transform.position = Vector3.Lerp(startPosition, finalPosition, _elapsed/_riseTime);

            _elapsed += Time.deltaTime;

            yield return null;
        }

        _lanceObject.transform.position = finalPosition;
    }
    
    private void AlternateTime(bool isTimeFrozen)
    {
        _isTimeFrozen = isTimeFrozen;
    }

    private void SwitchColliders(bool enable)
    {
        foreach (Collider2D collider in _colliders)
        {
            collider.enabled = enable;
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.limeGreen;
        Gizmos.DrawWireSphere(transform.position + transform.up * _startOffset, .4f);
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position + transform.up * _finalOffset, .4f);
    }
}