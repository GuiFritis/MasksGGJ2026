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

    [Header("Growth")]
    public Vector2 direction = Vector2.up;
    public float maxLength = 1f;

    private Vector3 _startScale;
    private Vector3 _endScale;
    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;

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
            yield return new WaitForSeconds(delay);

            yield return ScaleLance(_startScale, _endScale, growTime);
            col.enabled = true;

            yield return new WaitForSeconds(activeTime);

            col.enabled = false;
            yield return ScaleLance(_endScale, _startScale, shrinkTime);
        }
    }

    IEnumerator ScaleLance(Vector3 from, Vector3 to, float time)
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            float t = elapsed / time;
            transform.localScale = Vector3.Lerp(from, to, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = to;
    }
}
