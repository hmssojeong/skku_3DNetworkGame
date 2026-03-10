using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class PlayerVoiceAbility : PlayerAbility
{
    public GameObject SpeakingIcon;
    [SerializeField] private PhotonVoiceView _voiceView;
    private Recorder _recorder;

    private void Start()
    {
        _recorder = GetComponent<Recorder>();

        _recorder.VoiceDetection = true;
        _recorder.VoiceDetectionThreshold = 0.01f;
        _recorder.VoiceDetectionDelayMs = 300;
    }

    private void Update()
    {
        bool isSpeaking = false;

        if (_owner.PhotonView.IsMine)
        {
            isSpeaking = _recorder.IsCurrentlyTransmitting;
        }
        else
        {
            isSpeaking = _voiceView.IsSpeaking;
        }

        SpeakingIcon.SetActive(isSpeaking);

        if (Input.GetKeyDown(KeyCode.M))
        {
            // 음소거 토글
            _recorder.TransmitEnabled = !_recorder.TransmitEnabled;
        }
    }

}
