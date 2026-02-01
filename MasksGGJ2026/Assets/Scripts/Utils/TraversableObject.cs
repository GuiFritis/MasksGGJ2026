using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        
    }
}
