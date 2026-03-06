using Photon.Pun;
using UnityEngine;

public class GameScene : MonoBehaviourPunCallbacks
{

    private bool _isPlayerGenerated = false;

    private void Start()
    {
        if(PhotonNetwork.InRoom)
        {
            GenteratePlayer();
        }
    }


    public override void OnJoinedRoom()
    {
        GenteratePlayer();

    }


    private void GenteratePlayer()
    {
        if (_isPlayerGenerated) return;

        _isPlayerGenerated = true;

        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate("Object", Vector3.zero, Quaternion.identity);
        Debug.Log($"[GameScene] Player Instantiate 완료 | player null? {player == null} | ViewID: {player?.GetComponent<PhotonView>()?.ViewID}");
    }
}
