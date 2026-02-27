using UnityEngine;

public class MonsterController : MonoBehaviour
{

    public EMonsterState State = EMonsterState.Idle;

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
        }
    }

    private void Idle()
    {
        _animator.SetTrigger("Idle");
    }
}
