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
       

       /* if (!player.PhotonView.IsMine) return;

        
        if (photonView.Owner == player.PhotonView.Owner) return;*/
       



        _isCollected = true;
        //photonView.RPC(nameof(RPC_Collect), RpcTarget.AllViaServer, player.PhotonView.Owner.ActorNumber);
        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);


    }

    [PunRPC]
    private void RPC_Collect(int collectorActorNumber)
    {
        _isCollected = true;

        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            if (player.PhotonView.Owner.ActorNumber == collectorActorNumber)
            {
                if (player.PhotonView.IsMine)
                {
                    Debug.Log($"[ScoreItem] 점수 추가 → {player.gameObject.name}");
                    player.GetAbility<PlayerScoreAbility>().AddScore(1);
                }
                break;
            }
        }

        /*if (photonView.IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }*/

        ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
    }
}
