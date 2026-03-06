using UnityEngine;

public class PlayerWeaponColliderAbility : PlayerAbility
{
    [SerializeField] private Collider _collider;

    private void Start()
    {
        DeactiveCollider();
    }

    public void ActiveCollider()
    {
        _collider.enabled = true;
    }

    public void DeactiveCollider()
    {
        _collider.enabled = false;
    }

public void OnTriggerEnter(Collider other)
    {
        if (!_owner.PhotonView.IsMine) return;
        if (other.CompareTag("Player")) return;

        var damageable = other.GetComponent<IDamageable>();
        if (damageable == null) return;

        damageable.TakeDamage(_owner.Stat.Damage, transform.position, _owner.PhotonView.Owner.ActorNumber);
    }
}

