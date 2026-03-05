using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Lobby : MonoBehaviour
{
/*    [SerializeField] private Button _maleButton;
    [SerializeField] private Button _femaleButton;*/
    [SerializeField] private GameObject _maleCharacter;
    [SerializeField] private GameObject _femaleCharacter;

    public TMP_InputField NicknameInpuField;
    public InputField RoomNameInputField;
    public Button CreateRoomButton;

    private ECharacterType _characterType;

    public void OnClickMale() => OnClickeCharacterButton(ECharacterType.Male);
    public void OnClickFeMale() => OnClickeCharacterButton(ECharacterType.Male);

    private void OnClickeCharacterButton(ECharacterType characterType)
    {
        _characterType = characterType;

        _maleCharacter.SetActive(characterType == ECharacterType.Male);
        _femaleCharacter.SetActive(characterType == ECharacterType.Female);
    }
}
