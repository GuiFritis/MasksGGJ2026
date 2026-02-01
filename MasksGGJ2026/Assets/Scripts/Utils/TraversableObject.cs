using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TraversableObject : MonoBehaviour
{
    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        MaskSkillGhost.OnGhostActive += AlternateCollider;
    }

    private void OnDisable()
    {
        MaskSkillGhost.OnGhostActive -= AlternateCollider;
    }

    private void AlternateCollider(bool isGhost)
    {
        _collider.enabled = !isGhost;
    }    
}
