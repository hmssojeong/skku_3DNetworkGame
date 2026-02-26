using Photon.Pun;
using UnityEngine;

public class ItemObject : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;    

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Score += 100;

            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
