using UnityEngine;
using Photon.Pun;

public class MinimapIconSetup : MonoBehaviour
{
    [SerializeField] private Sprite _playerSprite;   // 내 아이콘 스프라이트
    [SerializeField] private Sprite _enemySprite;    // 적 아이콘 스프라이트
    [SerializeField] private Color _playerColor = Color.blue;
    [SerializeField] private Color _enemyColor = Color.red;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
private void Start()
    {
        PhotonView photonView = GetComponentInParent<PhotonView>();
        SpriteRenderer icon = GetComponent<SpriteRenderer>();

        if (photonView == null || icon == null) return;

        // IsMine은 Photon이 완전히 초기화된 뒤에 정확하므로 여기서 판단해도 안전
        if (photonView.IsMine)
        {
            icon.sprite = _playerSprite;
            icon.color = _playerColor;
        }
        else
        {
            icon.sprite = _enemySprite;
            icon.color = _enemyColor;
        }
    }

    // Update is called once per frame
void Update()
    {
    }
}
