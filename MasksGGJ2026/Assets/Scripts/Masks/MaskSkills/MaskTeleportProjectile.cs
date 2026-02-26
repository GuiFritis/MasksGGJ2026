using UnityEngine;

public class MaskTeleportProjectile : MonoBehaviour
{
    private PlayerMovement _player;

    [SerializeField]
    private float _lifeTime = 5f;

    [SerializeField]
    private LayerMask _translocatableLayers;
    [SerializeField] private AudioSO _audio;

    public void SetOwner(PlayerMovement player)
    {
        _player = player;
    }

    void Start()
    {
        Invoke(nameof(DestroySelf), _lifeTime);
    }

    void DestroySelf()
    {
        _player?.ClearProjectile();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_translocatableLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            if(collision.gameObject.TryGetComponent(out ITranslocatable translocatable))
            {
                Vector3 position = translocatable.TranslocatePosition();
                translocatable.SwitchPosition(_player.transform.position);
                _player.transform.position = position;
                _player.PlayerRigdbody.linearVelocity = Vector2.zero;
                SFX_Pool.Instance.Play(_audio);
                PlayerMaskManager.spendCharge?.Invoke();
                DestroySelf();
            }
        }
    }
}