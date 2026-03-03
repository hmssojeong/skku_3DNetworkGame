using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreItem : MonoBehaviour
{
    public TextMeshProUGUI NicknameTextUI;
    public TextMeshProUGUI ScoreTextUI;

    public void Set(string nickname, int score)
    {
        NicknameTextUI.text = nickname;
        ScoreTextUI.text = $"{score:N0}";
    }
}
