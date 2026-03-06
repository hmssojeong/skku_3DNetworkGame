using UnityEngine;

public class PlayerHitEffectAbility : PlayerAbility
{
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    public void OnHit(Vector3 attackerPosition)
    {
        HitAnimation();

        if (!_owner.PhotonView.IsMine) return;
        HitCameraEffect(attackerPosition);
    }

    private void HitCameraEffect(Vector3 attackerPosition)
    {
        if(_owner.PhotonView.IsMine)
        {
            Vector3 dir = (_owner.transform.position - attackerPosition).normalized;
            GetComponent<RecoilShake>().ScreenShake(dir);
        }
    }

    private void HitAnimation()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        if (_animator != null)
        {
            _animator.SetTrigger("OnHit");
        }
    }
}
