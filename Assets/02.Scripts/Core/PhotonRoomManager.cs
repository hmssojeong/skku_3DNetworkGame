using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance { get; private set; }

    private Room _room;
    public Room Room => _room;

    public event Action OnDataChanged;           // 룸 정보가 바뀌었을때
    public event Action<Player> OnPlayerEnter;   // 플레이거가 들어왔을떄
    public event Action<Player> OnPlayerLeft;    // 플레이어가 나갔을때
    public event Action<string, string> OnPlayerDeathed; // 플레이어가 죽었을때

    private void Awake()
    {
        Instance = this;
    }

    // 방 입장에 성공하면 자동으로 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;

        OnDataChanged?.Invoke();

        // 리소스 폴더에서 "Player" 이름을 가진 프리팹을 생성(인스턴스화)하고, 서버에 등록도한다.
        //   ㄴ 리소스 폴더는 나쁜것이다. 그러기 때문에 다른 방법을 찾아보거라..
        // 3줄 이상으로 하드코딩을 했다? 
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    // 새로운 플레이어가 방에 입장하면 자동으로 호출되는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnDataChanged?.Invoke();
        OnPlayerEnter?.Invoke(newPlayer);
    }

    // 플레이어가 방에서 퇴장하면 자동으로 호출되는 함수
    public override void OnPlayerLeftRoom(Player player)
    {
        OnDataChanged?.Invoke();
        OnPlayerLeft?.Invoke(player);
    }

    public void OnPlayerDeath(int attackerActorNumber, int victimActorNumber)
    {
        string attackerNickName = _room.Players[attackerActorNumber].NickName;
        string victimNickName = _room.Players[victimActorNumber].NickName;

        OnPlayerDeathed?.Invoke(attackerNickName, victimNickName);
    }
}