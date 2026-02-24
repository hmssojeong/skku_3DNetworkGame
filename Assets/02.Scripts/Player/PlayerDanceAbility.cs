using UnityEngine;
using Photon.Pun;

public class PlayerDanceAbility : PlayerAbility
{
    private Animator _animator;
    private bool _isDancing = false;
    private bool _isHipHopDancing = false;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _isDancing = !_isDancing;
            SetDanceState(_isDancing);
            _owner.PhotonView.RPC(nameof(SetDanceState), RpcTarget.Others, _isDancing);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _isHipHopDancing = !_isHipHopDancing;
            SetHipHopDanceState(_isHipHopDancing);
            _owner.PhotonView.RPC(nameof(SetHipHopDanceState), RpcTarget.Others, _isHipHopDancing);
        }
    }

    public void CancelDance()
    {
        if (!_isDancing && !_isHipHopDancing) return;

        _isDancing = false;
        _isHipHopDancing = false;

        SetDanceState(false);
        SetHipHopDanceState(false);
        _owner.PhotonView.RPC(nameof(SetDanceState), RpcTarget.Others, false);
        _owner.PhotonView.RPC(nameof(SetHipHopDanceState), RpcTarget.Others, false);
    }


    [PunRPC]
    private void SetDanceState(bool isDancing)
    {
        _animator.SetBool("IsDancing", isDancing);
    }

    [PunRPC]
    private void SetHipHopDanceState(bool isHipHopDancing)
    {
        _animator.SetBool("IsHipHopDancing", isHipHopDancing);
    }


}
