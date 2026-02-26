using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class TraversableObject : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Collider2D _collider;

    void OnValidate()
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

    private void AlternateCollider(bool active)
    {
        _collider.enabled = !active;
        _tilemap.color = active ? MaskSkillGhost.TRANSLUCID_WHITE : Color.white;
    }    
}
