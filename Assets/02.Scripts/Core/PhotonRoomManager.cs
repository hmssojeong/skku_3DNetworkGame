using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance { get; private set; }

    private Room _room;
    public Room Room => _room;

    public event Action OnDataChanged;
    public event Action<Player> OnPlayerEnter;
    public event Action<Player> OnPlayerLeft;
    public event Action<string, string> OnPlayerDeathed;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;

        Debug.Log($"[PhotonRoomManager] OnJoinedRoom | IsMasterClient: {PhotonNetwork.IsMasterClient} | PlayerCount: {_room.PlayerCount}");

        OnDataChanged?.Invoke();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleScene");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"[PhotonRoomManager] OnPlayerEnteredRoom | 새 플레이어: {newPlayer.NickName} (Actor: {newPlayer.ActorNumber}) | 현재 PlayerCount: {_room.PlayerCount}");

        // 3초 후에 씬 내 모든 PhotonView 목록 출력 (B의 Instantiate가 완료될 시간을 줌)
        StartCoroutine(LogAllPhotonViewsAfterDelay(3f));

        OnDataChanged?.Invoke();
        OnPlayerEnter?.Invoke(newPlayer);
    }

    private IEnumerator LogAllPhotonViewsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        PhotonView[] allViews = FindObjectsOfType<PhotonView>();
        Debug.Log($"[PhotonRoomManager] === 씬 내 모든 PhotonView ({allViews.Length}개) ===");

        foreach (var view in allViews)
        {
            Debug.Log($"  - ViewID: {view.ViewID} | Owner: {view.Owner?.NickName} | GameObject: {view.gameObject.name} | Active: {view.gameObject.activeInHierarchy} | Pos: {view.transform.position}");
        }

        // 모든 Player 태그 오브젝트도 확인
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log($"[PhotonRoomManager] === Player 태그 오브젝트 ({players.Length}개) ===");
        foreach (var p in players)
        {
            Debug.Log($"  - Name: {p.name} | Active: {p.activeInHierarchy} | Pos: {p.transform.position}");
        }
    }

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
