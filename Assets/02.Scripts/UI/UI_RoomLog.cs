using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RoomLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;

    private void Start()
    {
        _logText.text = "방에 입장했습니다.";

        PhotonRoomManager.Instance.OnPlayerEnter += OnPlayerEnterLog;
        PhotonRoomManager.Instance.OnPlayerLeft += OnPlayerLeftLog;
        PhotonRoomManager.Instance.OnPlayerDeathed += PlayerDeathLog;
    }

    private void OnPlayerEnterLog(Player newPlayer)
    {
        _logText.text += "\n" + $"{newPlayer.NickName}님이 입장하였습니다.";
    }

    private void OnPlayerLeftLog(Player player)
    {
        _logText.text += "\n" + $"{player.NickName}님이 퇴장하였습니다.";

    }

    private void PlayerDeathLog(string attackerNickName, string victimNickName)
    {
        _logText.text += "\n" + $"{attackerNickName}님이 {victimNickName}을 처치했습니다.";
    }
}