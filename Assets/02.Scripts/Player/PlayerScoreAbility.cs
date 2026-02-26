using Photon.Pun;
using UnityEngine;

public class PlayerScoreAbility : PlayerAbility, IPunObservable
{
    public int Score { get; private set; }

    public void AddScore(int amount)
    {
        _owner.PhotonView.RPC(nameof(RPC_AddScore), RpcTarget.All, amount);
    }

    [PunRPC]
    private void RPC_AddScore(int amount)
    {
        Score += amount;
        Debug.Log($"[PlayerScoreAbility] {gameObject.name} 점수 획득! 현재 점수: {Score}");
    }

    // 뒤늦게 방에 입장한 플레이어에게 현재 점수 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(Score);
        else
            Score = (int)stream.ReceiveNext();
    }
}
