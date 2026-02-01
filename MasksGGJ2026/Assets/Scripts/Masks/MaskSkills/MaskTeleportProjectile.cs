using UnityEngine;

public class MaskTeleportProjectile : MonoBehaviour
{
    private PlayerMovement _owner;

    [SerializeField]
    private float _lifeTime = 5f;

    [SerializeField]
    private LayerMask _translocatableLayers;

    public void SetOwner(PlayerMovement player)
    {
        _owner = player;
    }

    void Start()
    {
        Invoke(nameof(DestroySelf), _lifeTime);
    }

    void DestroySelf()
    {
        _owner?.ClearProjectile();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((_translocatableLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            var hitObject = collision.gameObject;
            TranslocatePlayer(hitObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_translocatableLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            var hitObject = collision.gameObject;
            TranslocatePlayer(hitObject);
        }
    }

    private void TranslocatePlayer(GameObject hitObject)
    {
        Vector3 setPlayerPosition = _owner.transform.position;
        Vector3 hitObjectPosition = hitObject.transform.position;

        _owner.transform.position = new Vector3(hitObjectPosition.x, setPlayerPosition.y);
        hitObject.transform.position = new Vector3(setPlayerPosition.x, hitObjectPosition.y);

        hitObject
            .GetComponent<ITranslocatableMovingObject>()?
            .ResetPosition();

        DestroySelf();
    }
}