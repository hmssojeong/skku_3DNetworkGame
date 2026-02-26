using Photon.Pun;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerWeaponHitAbility : PlayerAbility
{
private void OnTriggerEnter(Collider other)
    {
        if (!_owner.PhotonView.IsMine) return;

        if (other.transform == _owner.transform) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            PlayerController otherPlayer = other.GetComponent<PlayerController>();
            int attackerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

            otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage),RpcTarget.All,_owner.Stat.Damage,_owner.transform.position,attackerActorNumber);

            _owner.GetAbility<PlayerWeaponColliderAbility>().DeactiveCollider();
        }
    }
}