using System.Collections;
using Photon.Pun;
using UnityEngine;

public class ItemObjectSkyDrop : MonoBehaviour
{
    [SerializeField] private Transform[] _itemSpawnPoint;

    [SerializeField] private float _minDropTime;
    [SerializeField] private float _maxDropTime;

private void Start()
    {
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            if (_itemSpawnPoint == null || _itemSpawnPoint.Length == 0)
            {
                return;
            }
            if (_maxDropTime <= 0)
            {
                return;
            }
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
