using UnityEngine;

public class PlayerPhysicsMovement : MonoBehaviour
{
    public void SetCanMove(bool value) => canMove = value;
    public bool IsMoving { get { return inputDirection != Vector2.zero && canMove; } }
    public bool IsGrounded { get { return isGrounded; } }

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float groundDrag = 1f;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float airMultiplier = 0.4f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float jumpDelay = 0.15f;
    [SerializeField] private float groundDelay = 0.15f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    public bool isGrounded;
    private bool wasGrounded;

    private float groundTimer;
    private float jumpTimer;

    private bool canMove = true;
    private Vector2 inputDirection = Vector2.zero;
    private Vector3 moveDirection = Vector3.zero;

    [HideInInspector] public Rigidbody rigidBody;
    private InputService inputService;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        inputService = ServiceLocator.Instance.Get<InputService>();
    }

    private void Update()
    {
        Input();

        wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + .2f, groundLayers);

        if (wasGrounded && !isGrounded)
        {
            groundTimer = Time.time + groundDelay;
            wasGrounded = false;
        }

        if (isGrounded) { rigidBody.drag = groundDrag; }
        else { rigidBody.drag = 0; }

        SpeedControl();
    }

    private void FixedUpdate()
    {
        HorizontalMovement();

        if (jumpTimer > Time.time && (groundTimer > Time.time || isGrounded))
        {
            jumpTimer = 0;
            groundTimer = 0;
            Jump();
        }
    }

    private void Input()
    {
        inputDirection = inputService.playerInputActions.PlayerActionMap.Walk.ReadValue<Vector2>();

        if (inputService.playerInputActions.PlayerActionMap.Jump.IsPressed())
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }

    private void HorizontalMovement()
    {
        if (!canMove) { return; }

        moveDirection = cameraTransform.forward * inputDirection.y + cameraTransform.right * inputDirection.x;

        Vector3 moveForce = 10f * moveSpeed * moveDirection.normalized;
        if (!isGrounded) { moveForce *= airMultiplier; }

        rigidBody.AddForce(moveForce, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rigidBody.velocity = new Vector3(limitedVelocity.x, rigidBody.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        if (!canMove) { return; }

        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
