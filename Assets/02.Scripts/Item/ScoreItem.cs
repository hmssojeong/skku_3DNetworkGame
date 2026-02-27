using Photon.Pun;
using UnityEngine;

public class ScoreItem : MonoBehaviourPun
{
    [SerializeField] private float _collectDelay = 0.8f;

    private bool _isCollected = false;
    private float _spawnTime;

    private void Awake()
    {
        _spawnTime = Time.time;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomDir = new Vector3(
                Random.Range(-1f, 1f),
                1f,
                Random.Range(-1f, 1f)
            ).normalized;
            rb.AddForce(randomDir * 4f, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;

        var player = other.GetComponentInParent<PlayerController>();
        if (player == null) return;
       

        if (!player.PhotonView.IsMine) return;
        if (player.GetAbility<PlayerDeathAbility>().IsDead) return;

        _isCollected = true;
        //photonView.RPC(nameof(RPC_Collect), RpcTarget.AllViaServer, player.PhotonView.Owner.ActorNumber);
        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);

        player.GetAbility<PlayerScoreAbility>().AddScore(1);
        Debug.Log("점수추가");

    }
}
