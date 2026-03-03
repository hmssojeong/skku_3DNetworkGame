using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UI_Score : MonoBehaviour
{
    private List<UI_ScoreItem> _items;

    private void Start()
    {
        _items = GetComponentsInChildren<UI_ScoreItem>().ToList();

        ScoreManager.OnDataChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        var scores = ScoreManager.Instance.Scores;

        // 리드온니가 아니면 원본을 수정하므로 무결성 문제가 생긴다.

        List<ScoreData> scoresDatas = scores.Values
                                      .OrderByDescending(data => data.Score)
                                      .ToList();

        for(int i = 0; i < _items.Count; i++)
        {
            // 서버에 데이터가 있는 만큼만 UI를 보여주고, 나머지는 끄거나 비움
            if (i < scoresDatas.Count)
            {
                _items[i].gameObject.SetActive(true);
                _items[i].Set(scoresDatas[i].Nickname, scoresDatas[i].Score);
            }
            else
            {
                _items[i].gameObject.SetActive(false); // 플레이어가 없으면 UI 숨기기
            }
        }

    }
}
