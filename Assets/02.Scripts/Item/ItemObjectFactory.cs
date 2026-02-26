using Photon.Pun;
using UnityEngine;

public class ItemObjectFactory : MonoBehaviour
{

private void Awake()
    {
        Instance = this;
        _photonView = GetComponent<PhotonView>();
    }

    public static ItemObjectFactory Instance { get; private set; }
    private PhotonView _photonView;

    // 우리의 약속 : 방장에게 룸 관련해서 뭔가 요청을 할때는 메서드명에 Request로 시작하는게 유지보수가 편하다.

public void RequestMakeScoreItmes(Vector3 makePosition)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MakeScoreItems(makePosition);
        }
        else
        {
            _photonView.RPC(nameof(MakeScoreItems), RpcTarget.MasterClient, makePosition);
        }
    }

    [PunRPC]
    private void MakeScoreItems(Vector3 makePosition)
    {
        int count = UnityEngine.Random.Range(3, 6);

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), 0.5f, UnityEngine.Random.Range(-1.5f, 1.5f));
            PhotonNetwork.InstantiateRoomObject("ScoreItem", makePosition + offset, Quaternion.identity);
        }
    }

    public void RequestDelete(int viewId)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Delete(viewId);
        }
        else
        {
            _photonView.RPC(nameof(Delete),RpcTarget.MasterClient, viewId);
        }
    }

    [PunRPC]
    private void Delete(int viewId)
    {
        GameObject objectToDelete = PhotonView.Find(viewId)?.gameObject;
        if (objectToDelete == null) return;

        PhotonNetwork.Destroy(objectToDelete);
    }
}
