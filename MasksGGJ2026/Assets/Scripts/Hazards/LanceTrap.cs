using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpikeTrap : MonoBehaviour
{
    [Header("Configuration")]
    public bool isOdd;
    public float oddDelay = 1.5f;
    public float evenDelay = 3f;

    public float upTime = 0.5f;
    public float downTime = 0.5f;
    public float activeTime = 1f;

    [Header("Movement")]
    public Vector2 direction = Vector2.up;
    public float distance = 1f;

    private Vector3 startPos;
    private Vector3 endPos;
    private Collider2D col;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + (Vector3)(direction.normalized * distance);

        col = GetComponent<Collider2D>();
        col.enabled = false;

        StartCoroutine(SpikeRoutine());
    }

    IEnumerator SpikeRoutine()
    {
        float delay = isOdd ? oddDelay : evenDelay;

        while (true)
        {
            yield return new WaitForSeconds(delay);

            yield return MoveSpike(startPos, endPos, upTime);
            col.enabled = true;

            yield return new WaitForSeconds(activeTime);

            col.enabled = false;
            yield return MoveSpike(endPos, startPos, downTime);
        }
    }

    IEnumerator MoveSpike(Vector3 from, Vector3 to, float time)
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;
    }
}
