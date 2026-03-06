using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MonsterController : MonoBehaviour, IDamageable
{
    [SerializeField] private float _detectDistance = 5f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _returnDistance = 15f;

    [SerializeField] private float _attackPower = 2f;
    [SerializeField] private float _attackTimer = 0;
    [SerializeField] private float _attackSpeed = 2f;

    [SerializeField] private GameObject _player;

    private float _patrolRadius = 5f;
    private float _patrolDelay = 3f;
    private float _patrolDelayTime = 0f;
    private bool _isWaiting = false;
    private float _deathDelay = 6f;

    private MonsterStat _stat;

    private Vector3 _originPos;
    private int _currentPatrolIndex = 0;

    private List<Vector3> _monsterPatrolPoints = new List<Vector3>();

    public EMonsterState State = EMonsterState.Idle;

    private NavMeshAgent _agent;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _stat = GetComponent<MonsterStat>();
        _originPos = transform.position;

        TryFindPlayer();

        InitializePatrol();
        State = EMonsterState.Patrol;
        MoveToNextPatrolPoint();
    }

    private void TryFindPlayer()
    {
        if (_player == null || !_player.activeInHierarchy)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null && found.activeInHierarchy)
            {
                _player = found;
            }
            else
            {
                _player = null;
            }
        }
    }

    private void Update()
    {
        TryFindPlayer();
        _animator.SetFloat("Move", _agent.velocity.magnitude);

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
                break;

            case EMonsterState.Death:
                break;
        }
    }

    private void Idle()
    {
       // _animator.SetTrigger("Idle");
    }

    private void Trace()
    {
        if (_player == null)
        {
            State = EMonsterState.Patrol;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= _attackDistance)
        {
            State = EMonsterState.Attack;
            _attackTimer = _attackSpeed;
            return;
        }

        if (distanceToPlayer > _returnDistance)
        {
            State = EMonsterState.Comeback;
            return;
        }

        _agent.SetDestination(_player.transform.position);
    }

    private void Patrol()
    {
        if (_player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

            if (distanceToPlayer <= _detectDistance)
            {
                _isWaiting = false;
                State = EMonsterState.Trace;
                return;
            }
        }

        if(!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_isWaiting)
            {
                _isWaiting = true;
                _patrolDelayTime = 0f;
            }

            _patrolDelayTime += Time.deltaTime;
            if (_patrolDelayTime >= _patrolDelay)
            {
                _isWaiting = false;
                MoveToNextPatrolPoint();
            }
        }
    }

    private void MoveToNextPatrolPoint()
    {
        if (_monsterPatrolPoints.Count == 0) return;
        _agent.SetDestination(_monsterPatrolPoints[_currentPatrolIndex]);
        _currentPatrolIndex = (_currentPatrolIndex + 1) % _monsterPatrolPoints.Count;
    }

    private void InitializePatrol()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomPoint = _originPos + new Vector3(
                Random.Range(-_patrolRadius, _patrolRadius),
                0,
                Random.Range(-_patrolRadius, _patrolRadius)
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, _patrolRadius, NavMesh.AllAreas))
            {
                _monsterPatrolPoints.Add(hit.position);
            }
        }

        if (_monsterPatrolPoints.Count == 0)
        {
            _monsterPatrolPoints.Add(_originPos);
        }
    }

    private void Comeback()
    {
        _agent.SetDestination(_originPos);

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            State = EMonsterState.Patrol;
        }
    }

    private void Attack()
    {
        if (_player == null)
        {
            State = EMonsterState.Patrol;
            return;
        }

        _agent.ResetPath();

        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer > _attackDistance)
        {
            State = EMonsterState.Trace;
            return;
        }

        Vector3 direction = (_player.transform.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackSpeed)
        {
            _animator.SetTrigger("Attack");
            _attackTimer = 0f;
        }
    }

    public void OnAttackHit()
    {
        if (_player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        if (distanceToPlayer > _attackDistance * 1.5f) return;

        var playerController = _player.GetComponent<PlayerController>();
        if (playerController == null) return;

        playerController.PhotonView.RPC(
            nameof(PlayerController.TakeDamage),
            RpcTarget.All,
            _attackPower,
            transform.position,
            -1
        );
    }

    public void TakeDamage(float damage, Vector3 attackerPosition, int attackerActorNumber)
    {
        if (State == EMonsterState.Death) return;

        _stat.TakeDamage(damage);

        if (_stat.IsDead)
        {
            Death();
        }
        else
        {
            Hit();
            State = EMonsterState.Trace;
        }
    }

    private void Hit()
    {
        if (State == EMonsterState.Death) return;

        State = EMonsterState.Hit;
        _animator.SetTrigger("Hit");
        //State = EMonsterState.Trace;
    }

    private void Death()
    {
        State = EMonsterState.Death;
        _animator.SetTrigger("Death");
        _agent.isStopped = true;
        Destroy(gameObject, _deathDelay);
    }
}
