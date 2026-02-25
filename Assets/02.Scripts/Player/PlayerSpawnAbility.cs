using UnityEngine;
using System.Collections;

public class PlayerSpawnAbility : PlayerAbility
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _respawnTime;

private void Start()
    {
        GameObject spawnPointsRoot = GameObject.Find("SpawnPoints");
        _spawnPoints = new Transform[spawnPointsRoot.transform.childCount];
        for (int i = 0; i < spawnPointsRoot.transform.childCount; i++)
        {
            _spawnPoints[i] = spawnPointsRoot.transform.GetChild(i);
        }

        Transform minimapCamTransform = _owner.transform.Find("MinimapCamera");
        if (minimapCamTransform != null)
        {
            if (_owner.PhotonView.IsMine)
            {
                Camera cam = minimapCamTransform.GetComponent<Camera>();
                UIMinimap uiMinimap = FindObjectOfType<UIMinimap>();
                if (uiMinimap != null && cam != null)
                    uiMinimap.SetMinimapCamera(cam);
            }
            else
            {
                // 다른 플레이어의 MinimapCamera는 끌서 레더링 추돉 방지
                minimapCamTransform.gameObject.SetActive(false);
            }
        }

        if (!_owner.PhotonView.IsMine) return;

        Spawn();
    }

    public void Spawn()
    {
        Transform point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        CharacterController characterControll = _owner.GetComponent<CharacterController>();
        characterControll.enabled = false;

        transform.position = point.position;
        characterControll.enabled = true;
    }

    public void RespawnAfterDelay()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(_respawnTime);

        _owner.GetAbility<PlayerDeathAbility>().Revive();

        if (_owner.PhotonView.IsMine)
        {
            Spawn();
        }
    }
}
