using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LanceTrap : MonoBehaviour
{
    [Header("Timing")]
    public bool isOdd;
    public float oddDelay = 1.5f;
    public float evenDelay = 3f;

    public float growTime = 0.5f;
    public float shrinkTime = 0.5f;
    public float activeTime = 1f;

    private float _elapsed;

    private bool _isTimeFrozen;

    [Header("Growth")]
    public Vector2 direction = Vector2.up;
    public float maxLength = 1f;

    private Vector3 _startScale;
    private Vector3 _endScale;
    
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
        direction = direction.normalized;

        _startScale = transform.localScale;

        _endScale = _startScale + new Vector3(
            direction.x * maxLength,
            direction.y * maxLength,
            0
        );

        StartCoroutine(LanceRoutine());
    }

    IEnumerator LanceRoutine()
    {
        float delay = isOdd ? oddDelay : evenDelay;
        
        while (true)
        {    
            yield return WaitTime(delay);

            yield return ScaleLance(_startScale, _endScale, growTime);

            yield return WaitTime(activeTime);

            yield return ScaleLance(_endScale, _startScale, shrinkTime);
        }
    }

    IEnumerator ScaleLance(Vector3 from, Vector3 to, float time)
    {
        _elapsed = 0f;

        while (_elapsed < time)
        {
            yield return new WaitWhile(() => _isTimeFrozen);
            
            float t = _elapsed / time;
            transform.localScale = Vector3.Lerp(from, to, t);

            _elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;
    }

    IEnumerator WaitTime(float seconds)
    {
        float elapsed = 0f;

        while (elapsed < seconds)
        {
            yield return new WaitWhile(() => _isTimeFrozen);

            elapsed += Time.deltaTime;
            
            yield return null;
        }
    }
    
    private void AlternateTime(bool isTimeFrozen)
    {
        _isTimeFrozen = isTimeFrozen;
    }    
}