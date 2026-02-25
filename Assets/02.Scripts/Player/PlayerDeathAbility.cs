using UnityEngine;

public class PlayerDeathAbility : PlayerAbility
{
    [SerializeField] private float _fallDeathY = -10f; // 이 Y값 이하로 떨어지면 낙사


    public bool IsDead { get; private set; }
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (IsDead) return;

        if (transform.position.y < _fallDeathY)
        {
            Die();
        }
    }


public void Die()
    {
        if (IsDead) return;
        IsDead = true;

        _animator.SetBool("IsDead", true);

        _owner.GetAbility<PlayerMoveAbility>().enabled = false;
        _owner.GetAbility<PlayerAttackAbility>().enabled = false;

        _owner.GetAbility<PlayerSpawnAbility>().RespawnAfterDelay();
    }

    public void Revive()
    {
        IsDead = false;
        _owner.Stat.Health = _owner.Stat.MaxHealth;

        // Animator를 완전히 초기 상태(Idle)로 리셋
        _animator.Rebind();
        _animator.Update(0f);

        _owner.GetAbility<PlayerMoveAbility>().enabled = true;
        _owner.GetAbility<PlayerAttackAbility>().enabled = true;
    }

}
