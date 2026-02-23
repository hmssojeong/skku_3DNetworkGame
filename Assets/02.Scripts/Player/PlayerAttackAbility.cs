using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    private Animator _animator;

    private float _attackTimer = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!_owner.PhotonView.IsMine)
        {
            return;
        }

        _attackTimer += Time.deltaTime;

        if(Input.GetMouseButton(0) && _attackTimer >= (1f / _owner.Stat.AttackCoolTime))
        {
            _attackTimer = 0f;

            _animator.SetTrigger($"Attack{Random.Range(1,4)}");
        }
    }
}
