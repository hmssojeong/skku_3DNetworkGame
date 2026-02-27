using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{ 
    [SerializeField] private float _detectDistance = 5f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _attackPower = 2f;
    [SerializeField] private GameObject _player;

    //private Tranform[] _monsterPatrolPoint;

    public EMonsterState State = EMonsterState.Idle;

    private NavMeshAgent _agent;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch(State)
        {
            case EMonsterState.Idle:
                Idle();
                break;

            case EMonsterState.Trace:
                Trace();
                break;

            case EMonsterState.Patrol:
                Patrol();
                break;

            case EMonsterState.Comeback:
                Comeback();
                break;

            case EMonsterState.Attack:
                Attack();
                break;

            case EMonsterState.Hit:
                Hit();
                break;

            case EMonsterState.Death:
                Death();
                break;
        }
    }

    private void Idle()
    {
        _animator.SetTrigger("Idle");
    }

    private void Trace()
    {
        float move = _agent.velocity.magnitude;
        _animator.SetFloat("Move", move);

        float distance = Vector3.Distance(_player.transform.position, transform.position);

        if (distance > _detectDistance)
        {
            Comeback();
        }
    }

    private void Patrol()
    {

        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance <= _detectDistance)
        {
            Trace();
        }
    }

    private void Comeback()
    {
        float move = _agent.velocity.magnitude;
        _animator.SetFloat("Move", move);

        //if(transform.position == Patrol.points)
        {
            Patrol();
        }
    }

    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if(distance <= _attackDistance)
        {
            //TakeDamage(1,transform.position, _attackPower);
        }
    }

    private void Hit()
    {

    }

    private void Death()
    {

    }
}
