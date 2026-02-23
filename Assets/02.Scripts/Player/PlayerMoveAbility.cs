using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float JumpForce = 2.5f;
    private const float GRAVITY = 9.8f;
    private float _yVeocity = 0f;   

    private CharacterController _characterController;

    // 1. 중력을 적용하세요.
    // 2. 스페이스바를 누르면 점프하게 해주세요.
    // 3. 플레이어 이동을 카메라가 바라보는 방향 기준으로 해주세요.
    // 4. Idle/Run 애니메이션을 블렌드로 적용해주세요.
    // 5. PlayerAttackAbility 스크립트를 만들어서
    //      - 마우스 왼쪽 클릭시마다 Attack1 / Attack2 / Attack3 애니메이션이 아래 옵션에 따라 실행시켜주세요.
    //      - 열거형 옵션 1. 순차적
    //      -        옵션 2. 랜덤

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 1. 중력 적용
        if (_characterController.isGrounded)
        {
            _yVeocity = 0f;
        }
        else
        {
            _yVeocity -= GRAVITY * Time.deltaTime;
        }

        // 2. 스페이스바 점프
        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVeocity = JumpForce;
        }

        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = _yVeocity;
        _characterController.Move(velocity * Time.deltaTime);
    }
}
