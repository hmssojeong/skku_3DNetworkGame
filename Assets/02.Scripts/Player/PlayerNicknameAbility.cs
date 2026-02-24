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

// 1. 체력과 스태미나 UI를 구성하세요.
// 2. Shift 키를 누르고 있는 동안 스태미나가 있다면 이동속도 up!
// 3. 아니라면 스태미나를 회복해주세요.
