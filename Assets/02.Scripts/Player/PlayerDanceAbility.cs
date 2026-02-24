using UnityEngine;
using Photon.Pun;

public class PlayerDanceAbility : PlayerAbility
{
    private Animator _animator;
    private bool _isDancing = false;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 내 캐릭터만 입력 처리
        if (!_owner.PhotonView.IsMine) return;

        // 키보드 1번 누를 때 댄스 토글
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _isDancing = !_isDancing;

            _animator.SetBool("IsDancing", _isDancing);

            if (_isDancing)
                Debug.Log("[DanceAbility] 댄스 시작!");
            else
                Debug.Log("[DanceAbility] 댄스 종료!");
        }
    }
}
