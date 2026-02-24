using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceTrap : MonoBehaviour, ITranslocatable
{
    [SerializeField] private GameObject _lanceObject;
    [SerializeField] private List<Collider2D> _colliders = new();
    [Header("Timing")]
    [SerializeField] private bool _isOdd;
    [SerializeField] private float _delay = 1.5f;
    [SerializeField] private float _riseTime = 0.5f;
    [SerializeField] private Vector3 _translocateOffset;
    private static readonly WaitForSeconds WAIT_0_01 = new(0.01f); 
    private float _elapsed;
    private bool _isTimeFrozen;
    private bool _isGhostActive;

    [Header("Rise")]
    [SerializeField] private float _startOffset = 1f;
    [SerializeField] private float _finalOffset = 1f;
    
    private void OnEnable()
    {
        MaskSkillFreezeTime.OnFreezeTime += AlternateTime;
        MaskSkillGhost.OnGhostActive += GhostMode;
    }

    private void OnDisable()
    {
        MaskSkillFreezeTime.OnFreezeTime -= AlternateTime;
        MaskSkillGhost.OnGhostActive -= GhostMode;
    }

    void Start()
    {
        StartCoroutine(LanceRoutine());
    }

    IEnumerator LanceRoutine()
    {
        if(_isOdd)
        {
            yield return new WaitForSeconds(_delay + _riseTime + 0.02f);
        }
        while (true)
        {    
            yield return Rise(
                transform.position + transform.up * _startOffset, 
                transform.position + transform.up * _finalOffset
            );

            yield return Delay();

            yield return Rise(
                transform.position + transform.up * _finalOffset,
                transform.position + transform.up * _startOffset
            );

            SwitchColliders(false);

            yield return Delay();
        }
    }

    IEnumerator Rise(Vector3 startPosition, Vector3 finalPosition)
    {
        _elapsed = 0f;

        while (_elapsed < _riseTime)
        {
            if(_isTimeFrozen)
            {
                yield return new WaitWhile(() => _isTimeFrozen);
            }

            _lanceObject.transform.position = Vector3.Lerp(startPosition, finalPosition, _elapsed/_riseTime);

            _elapsed += 0.01f;

            yield return WAIT_0_01;

            if(!_isGhostActive)
            {
                SwitchColliders(true);
            }
        }

        _lanceObject.transform.position = finalPosition;
    }

    IEnumerator Delay()
    {
        _elapsed = 0f;
        while(_elapsed < _delay)
        {
            if(_isTimeFrozen)
            {
                yield return new WaitWhile(() => _isTimeFrozen);
            }
            _elapsed += 0.01f;
            yield return WAIT_0_01;
        }
    }
    
    private void AlternateTime(bool isTimeFrozen)
    {
        _isTimeFrozen = isTimeFrozen;
    }

    private void GhostMode(bool active)
    {
        _isGhostActive = active;
        if(_isGhostActive)
        {
            SwitchColliders(false);
        }
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + _translocateOffset, .2f);
    }

    public Vector3 TranslocatePosition()
    {
        return transform.position + _translocateOffset;
    }

    public void SwitchPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}