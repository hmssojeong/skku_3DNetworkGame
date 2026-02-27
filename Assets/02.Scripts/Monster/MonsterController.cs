using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{ 
    [SerializeField] private float _detectDistance = 5f;
    [SerializeField] private float _attackDistance = 2f;

    [SerializeField] private float _attackPower = 2f;
    [SerializeField] private float _attackTimer = 0;
    [SerializeField] private float _attackSpeed = 2f;

    [SerializeField] private GameObject _player;

    private float _patrolRadius = 3f;
    private float _patrolNearby = 2f;

    private Vector3 _originPos;
    private int _currentPatrolIndex = 0;

    List<Vector3> _monsterPatrolPoint;

    public EMonsterState State = EMonsterState.Idle;

    private NavMeshAgent _agent;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        InitializePatrol();
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
            State = EMonsterState.Comeback;
        }
    }

    private void Patrol()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance <= _detectDistance)
        {
            State = EMonsterState.Trace;
        }
    }

    private void InitializePatrol()
    {
        List<Vector3> _possiblePatrolPoint = new List<Vector3>
        {
            // x 좌우 z 앞뒤
            new Vector3(0,0,0),
            new Vector3(_patrolRadius, 0, _patrolRadius),
            new Vector3(-_patrolRadius, 0, -_patrolRadius),
            new Vector3(_patrolRadius, 0, -_patrolRadius),
            new Vector3(-_patrolRadius, 0, _patrolRadius),
            new Vector3(_patrolRadius * _patrolNearby, 0, 0),
            new Vector3(-_patrolRadius * _patrolNearby, 0, 0),
            new Vector3(0, 0, _patrolRadius * _patrolNearby),
            new Vector3(0, 0, -_patrolRadius * _patrolNearby),

        };

        _monsterPatrolPoint = new List<Vector3>();
        List<Vector3> possiblePointCopy = new List<Vector3>();

        for (int i = 0; i < _monsterPatrolPoint.Count; i++)
        {
            int randomIndex = Random.Range(0, _monsterPatrolPoint.Count);
            Vector3 worldPosition = _originPos + possiblePointCopy[randomIndex];
            _monsterPatrolPoint.Add(worldPosition);
            possiblePointCopy.RemoveAt(randomIndex);
        }

        _currentPatrolIndex = 0;
    }

    private void Comeback()
    {
        // _monsterPatrolPoint[0] 이곳으로 돌아오기

        float move = _agent.velocity.magnitude;
        _animator.SetFloat("Move", move);

        if (transform.position == _monsterPatrolPoint[0])
        {
            State = EMonsterState.Patrol;
        }
    }

    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if(distance <= _attackDistance)
        {
            State = EMonsterState.Attack;
            //TakeDamage(1,transform.position, _attackPower);
            _animator.SetTrigger("Attack");

            _attackTimer += Time.deltaTime;
        }
        else
        {
            State = EMonsterState.Trace;

        }
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer > _attackSpeed)
        {
            _attackTimer = 0;

            //TakeDamage(1,transform.position, _attackPower);
            _animator.SetTrigger("Attack");          
        }
    }

    private void Hit()
    {
        _animator.SetTrigger("Hit");

    }

    private void Death()
    {
        _animator.SetTrigger("Death");
    }
}
