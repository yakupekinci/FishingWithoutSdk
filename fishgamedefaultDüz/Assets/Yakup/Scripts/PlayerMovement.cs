using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : Singleton<PlayerMovement>
{

    private CharacterController _controller;
    private Animator _animator;
    [SerializeField] private Float playerSpeed;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private float _turnSpeed = 10.0f;
    [SerializeField] private float _groundedRayLength = 1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] GameObject dustParticle;

    private float _gravity = 9.81f;
    public Vector3 _velocity = new Vector3(0, -9.8f, 0);
    Vector3 moveDirection;
    bool isGrounded;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        idleMidrollTimer = rollTime;
    }

    private void Update()
    {
        HandleMovement();
    }

    public void EnableMovement()
    {
        enabled = true;
    }

    public void DisableMovement()
    {
        _animator.SetBool("isRunning", false);
        enabled = false;
    }

    float idleMidrollTimer;
    bool hasShown;
    [SerializeField] float rollTime = 180f;

    private void ResetTimer()
    {
        idleMidrollTimer = rollTime;
        hasShown = false;
    }

    private void ErrorTimer()
    {
        hasShown = false;
    }

    private void HandleMovement()
    {
        float horizontalInput = _joystick.Horizontal;
        float verticalInput = _joystick.Vertical;

        if (horizontalInput == 0 && verticalInput == 0)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            RotateTowardsDirection(moveDirection);
            dustParticle.SetActive(true);
            idleMidrollTimer = rollTime;
        }
        else
        {
            idleMidrollTimer -= Time.deltaTime;
            if (idleMidrollTimer <= 0f && !hasShown)
            {
                hasShown = true;
               /*AdManager.Instance.ShowMidRoll(ResetTimer, ErrorTimer);*/
            }
            dustParticle.SetActive(false);
        }

        isGrounded = CheckGrounded();

        if (isGrounded)
        {
            if (moveDirection != Vector3.zero)
            {
                RotateTowardsDirection(moveDirection);
                MoveCharacter(moveDirection);
                _animator.SetBool("isRunning", true);
            }
            else
            {
                _animator.SetBool("isRunning", false);
            }

        }
        else
        {
            ApplyGravity();
            _animator.SetBool("isRunning", false);
            _velocity.y -= _gravity * Time.deltaTime;
        }
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, _turnSpeed * Time.deltaTime);
    }

    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _groundedRayLength, _groundLayer);
    }

    private void MoveCharacter(Vector3 moveDirection)
    {
        Vector3 movement = moveDirection * playerSpeed.Value * Time.deltaTime;
        _controller.Move(movement);
        _animator.SetBool("isRunning", true);
    }

    private void ApplyGravity()
    {
        _velocity.y = -_gravity; // Yerçekimi uygula
        _controller.Move(_velocity * Time.deltaTime);
    }
}
