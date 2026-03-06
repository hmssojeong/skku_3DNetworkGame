using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Lobby : MonoBehaviour
{
    public GameObject MaleCharacter;
    public GameObject FemaleCharacter;

    public TMP_InputField NicknameInputField;
    public TMP_InputField RoomNameInputField;
    public Button CreateRoomButton;

    private ECharacterType _characterType;

    private void Start()
    {
        CreateRoomButton.onClick.AddListener(MakeRoom);
    }

    private void MakeRoom()
    {
        // 서버에 연결되어 있고, 로비에 입장한 상태인지 확인
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("[UI_Lobby] 아직 서버에 연결되지 않았습니다. 잠시 후 다시 시도해주세요.");
            return;
        }

        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning("[UI_Lobby] 아직 로비에 입장하지 않았습니다. 잠시 후 다시 시도해주세요.");
            return;
        }

        string nickname = NicknameInputField.text;
        string roomName = RoomNameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            return;
        }

        PhotonNetwork.NickName = nickname;

        // 룸 옵션 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;  // 룸 최대 접속자 수
        roomOptions.IsVisible = true; // 로비에서 룸을 보여줄 것인지
        roomOptions.IsOpen = true;    // 룸의 오픈 여부

        Debug.Log($"[UI_Lobby] 방 만들기 시도 | 방 이름: {roomName} | 닉네임: {nickname}");

        // 룸 만들기 
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    // Todo: 버튼 연결
    public void OnClickMale() => OnClickCharacterButton(ECharacterType.Male);
    public void OnClickFemale() => OnClickCharacterButton(ECharacterType.Female);
    private void OnClickCharacterButton(ECharacterType characterType)
    {
        _characterType = characterType;

        MaleCharacter.SetActive(_characterType == ECharacterType.Male);
        FemaleCharacter.SetActive(_characterType == ECharacterType.Female);
    }
}
