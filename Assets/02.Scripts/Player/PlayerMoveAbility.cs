using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private const float GRAVITY = 9.8f;
    private float _yVelocity = 0f;   

    private CharacterController _characterController;

    private Animator _animator;

    // 1. 중력을 적용하세요.
    // 2. 스페이스바를 누르면 점프하게 해주세요.
    // 3. 플레이어 이동을 카메라가 바라보는 방향 기준으로 해주세요.
    // 4. Idle/Run 애니메이션을 블렌드로 적용해주세요.
    // 5. PlayerAttackAbility 스크립트를 만들어서
    //      - 마우스 왼쪽 클릭시마다 Attack1 / Attack2 / Attack3 애니메이션이 아래 옵션에 따라 실행시켜주세요.
    //      - 열거형 옵션 1. 순차적
    //      -        옵션 2. 랜덤

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 내꺼가 아니면 건들지 않는다!
        if(!_owner.PhotonView.IsMine)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 카메라가 바라보는 방향 기준으로 수정하기
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);

        _animator.SetFloat("Move", moveDirection.magnitude);

        // 1. 중력 적용
        if (_characterController.isGrounded)
        {
            _yVelocity = 0f;
        }
        else
        {
            _yVelocity -= GRAVITY * Time.deltaTime;
        }

        // 2. 스페이스바 점프
        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = _owner.Stat.JumpPower;
        }

        Vector3 velocity = moveDirection * _owner.Stat.MoveSpeed;
        velocity.y = _yVelocity;
        _characterController.Move(velocity * Time.deltaTime);
    }
}
