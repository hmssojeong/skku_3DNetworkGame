using System.Collections;
using Photon.Pun;
using UnityEngine;

public class ItemObjectSkyDrop : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _itemSpawnPoint;

    [SerializeField] private float _minDropTime;
    [SerializeField] private float _maxDropTime;

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_itemSpawnPoint == null || _itemSpawnPoint.Length == 0) return;
        if (_maxDropTime <= 0) return;

        _minDropTime = Mathf.Max(_minDropTime, 0.5f);
        StartCoroutine(SkyDropRoutine());
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _minDropTime = Mathf.Max(_minDropTime, 0.5f);
            StartCoroutine(SkyDropRoutine());
        }
    }

    private IEnumerator SkyDropRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(_minDropTime, _maxDropTime);
            yield return new WaitForSeconds(waitTime);
            SkyDropItem();
        }
    }

    private void SkyDropItem()
    {
        if (_itemSpawnPoint == null || _itemSpawnPoint.Length == 0)
        {
            return;
        }

        Transform itemDropPosition = _itemSpawnPoint[Random.Range(0, _itemSpawnPoint.Length)];
        PhotonNetwork.InstantiateRoomObject("ScoreItem", itemDropPosition.position, Quaternion.identity);
    }
}
