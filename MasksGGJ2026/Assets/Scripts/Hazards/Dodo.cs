using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Dodo : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _originalPosition;
    [SerializeField] private float _recoveryTime;
    [SerializeField] private float _knockdown;
    [SerializeField] private AudioSO _audioSO;
    private bool _isTimeFrozen;
    private bool _isRecovering = false;

    void OnValidate()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        } 
        if (_originalPosition == null)
        {
            _originalPosition = transform.position;
        }
    }

    private void OnEnable()
    {
        MaskSkillFreezeTime.OnFreezeTime += AlternateTime;
    }

    private void OnDisable()
    {
        MaskSkillFreezeTime.OnFreezeTime -= AlternateTime;
    }

    public void HandlePogo()
    {        
        if (!_isRecovering)
        {
            SFX_Pool.Instance.Play(_audioSO);

            _originalPosition = _rigidbody.position;

            _rigidbody.AddForce(_knockdown * Vector2.down, ForceMode2D.Impulse);

            _isRecovering = true;

            StartCoroutine(PogoRecovery());   
        }
    }

    IEnumerator PogoRecovery()
    {        
        yield return new WaitForSeconds(.15f);
        _rigidbody.linearVelocityY = 0;

        Vector2 from = _rigidbody.position;
        Vector2 to = _originalPosition;

        float elapsed = 0f;


        while (elapsed < _recoveryTime)
        {
            yield return new WaitWhile(() => _isTimeFrozen);

            float t = elapsed / _recoveryTime;
            _rigidbody.MovePosition(Vector2.Lerp(from, to, t));

            elapsed += Time.deltaTime;
            yield return null;
        }

        _rigidbody.MovePosition(to);

        _isRecovering = false;
    }
    
    private void AlternateTime(bool isTimeFrozen)
    {
        _isTimeFrozen = isTimeFrozen;
    }    
}