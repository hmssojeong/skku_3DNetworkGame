using UnityEngine;
using TMPro;
public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;

    private void Start()
    {
        _nicknameTextUI.text = _owner.PhotonView.Owner.NickName;

        if (_owner.PhotonView.IsMine)
        {
            _nicknameTextUI.color = new Color32(127, 244, 25, 255);
        }
        else
        {
            _nicknameTextUI.color= Color.red;
        }
    }

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
