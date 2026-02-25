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

            otherPlayer.PhotonView.RPC(
                nameof(damageable.TakeDamage),
                RpcTarget.All,
                _owner.Stat.Damage,
                _owner.transform.position,
                attackerActorNumber  // 누락됐던 파라미터 추가
            );

            _owner.GetAbility<PlayerWeaponColliderAbility>().DeactiveCollider();
        }
    }
}


/*//damageable.TakeDamage(_owner.Stat.Damage);

// 포톤에서는 Room 안에서 플레이어마다 고유 식별자(ID)인 ActorNumber를 가지고 있다.
int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
// int actorNumber = _owner.PhotonView.Owner.ActorNumber;

// 상대방의 TakeDamage를 RPC로 호출한다.
PlayerController otherPlayer = other.GetComponent<PlayerController>();
otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.Damage, _owner.transform.position);*/