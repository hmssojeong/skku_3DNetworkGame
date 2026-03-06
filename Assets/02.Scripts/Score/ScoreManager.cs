using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance { get; private set; }

    private int _score;

    private Dictionary<int, ScoreData> _scores = new();
    public ReadOnlyDictionary<int, ScoreData> Scores => new ReadOnlyDictionary<int, ScoreData>(_scores);
    //public Dictionary<int ,ScoreData> Scores => _scores;
    // 외부에서 수정 못하게 ReadonlyDictionary로 반환..

    public static event Action OnDataChanged;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            // 내 초기 점수 등록
            Refresh();

            // 이미 방에 있는 다른 플레이어들의 점수를 읽어와서 등록
            // (OnPlayerPropertiesUpdate는 변경 시에만 호출되므로, 뒤늦게 합류하면 기존 점수를 못 받음)
            foreach (var kvp in PhotonNetwork.CurrentRoom.Players)
            {
                Player player = kvp.Value;
                if (player.CustomProperties.ContainsKey("score"))
                {
                    ScoreData scoreData = new ScoreData()
                    {
                        Nickname = player.NickName,
                        Score = (int)player.CustomProperties["score"]
                    };
                    _scores[player.ActorNumber] = scoreData;
                }
            }

            OnDataChanged?.Invoke();
        }
    }

    public void AddScore(int score)
    {
        _score += score;

        Refresh();
    }

    private void Refresh()
    {
        // 해시테이블은 딕셔너리와 같은 키-값 형태로 저장하는데..
        //                              키-값에 있어서 자료형이 object다.
        Hashtable hashtable = new Hashtable();
        hashtable.Add("score", _score);

        // 프로퍼티 등록
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    // 플레이어의 커스텀 프로퍼티가 변경되면 자동으로 호출되는 함수
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("score")) return;

        ScoreData scoreData = new ScoreData()
        {
            Nickname = targetPlayer.NickName,
            Score = (int)changedProps["score"]
        };

        _scores[targetPlayer.ActorNumber] = scoreData;

        OnDataChanged?.Invoke();
    }

    // [데이터 공유]
    // 1. OnSerializwView (+TransformView, AnimatorView, ...)
    //     ㄴ C# 기본 타입, Vector, ..
    //     ㄴ PhotonNetwork... Rate... 에 따라

    // 2. RPC -> 매개변수를 활용해서 데이터 동기화
    //      ㄴ 주로 변화가 빈번하지 않은 데이터를 함수 호출을 이용해서 동기화..

    // 3. 커스텀프로퍼티(Custom Property)
    //    ㄴ 주로 변화가 빈번하지 않은 데이터들을 해시 테이블로 동기화...
    //    ㄴ (플레이어 준비 상태, 점수, 룸의 모드, 맵 선택 등..)


}
