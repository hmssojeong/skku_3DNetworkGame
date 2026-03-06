using Photon.Pun;
using UnityEngine;

public class GameScene : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate("Object", Vector3.zero, Quaternion.identity);
        Debug.Log($"[GameScene] Player Instantiate 완료 | player null? {player == null} | ViewID: {player?.GetComponent<PhotonView>()?.ViewID}");
    }
}
