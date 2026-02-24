using UnityEngine;
using Photon.Pun;

public class MinimapIconSetup : MonoBehaviour
{
    [SerializeField] private Sprite _playerSprite;   // 내 아이콘 스프라이트
    [SerializeField] private Sprite _enemySprite;    // 적 아이콘 스프라이트
    [SerializeField] private Color _playerColor = Color.blue;
    [SerializeField] private Color _enemyColor = Color.red;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        PhotonView photonView = GetComponentInParent<PhotonView>();
        SpriteRenderer icon = GetComponent<SpriteRenderer>();

        if (photonView == null || icon == null) return;

        if (photonView.IsMine)
        {
            icon.sprite = _playerSprite;  // 나 -> 파란 아이콘
            icon.color = _playerColor;
        }
        else
        {
            icon.sprite = _enemySprite;   // 상대방 -> 빨간 아이콘
            icon.color = _enemyColor;
        }
    }

    // Update is called once per frame
void Update()
    {
    }
}
