using Photon.Pun;
using UnityEngine;

public class PlayerScoreAbility : PlayerAbility, IPunObservable
{
    public int Score { get; private set; }

    [SerializeField] private Transform _weaponTransform;

    private Vector3 _baseWeaponScale;

    private const float ScalePerThreshold = 0.1f;
    private const int ScoreThreshold = 1000;

    protected override void Awake()
    {
        base.Awake();

        if (_weaponTransform != null)
        {
            if (_weaponTransform.localScale == Vector3.zero)
                _baseWeaponScale = Vector3.one;
            else
                _baseWeaponScale = _weaponTransform.localScale;
        }
    }

    public void AddScore(int amount)
    {
        _owner.PhotonView.RPC(nameof(RPC_AddScore), RpcTarget.All, amount);
    }

    [PunRPC]
    private void RPC_AddScore(int amount)
    {
        Score += amount;
        Debug.Log($"[PlayerScoreAbility] {gameObject.name} 점수 획득! 현재 점수: {Score}");

        UpdateWeaponScale();

        if(photonView.IsMine)
        {
            ScoreManager.Instance.AddScore(amount);
        }
    }

    // 뒤늦게 방에 입장한 플레이어에게 현재 점수 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Score);
        }         
        else
        {
            Score = (int)stream.ReceiveNext();
            UpdateWeaponScale();
        }           
    }

    private void UpdateWeaponScale()
    {
        if (_weaponTransform == null) return;

        if (_baseWeaponScale == Vector3.zero) _baseWeaponScale = Vector3.one;

        int level = Score / ScoreThreshold;
        float scaleMultiplier = 1f + (level * ScalePerThreshold);
        _weaponTransform.localScale = _baseWeaponScale * scaleMultiplier;
    }

    public void PenaltyScoreOnDeath()
    {
        if(photonView.IsMine)
        {
            int penalty = Score / 2;
            photonView.RPC(nameof(RPC_AddScore), RpcTarget.All, -penalty);
        }
    }
}
