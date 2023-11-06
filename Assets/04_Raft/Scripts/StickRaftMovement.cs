using UnityEngine;
using UnityEngine.InputSystem;

public class StickRaftMovement : MonoBehaviour
{
    [Header("Raft Settings")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private bool jumpToDisconnect;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private bool controllingRaft = false;
    private Vector2 inputDirection = Vector2.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 playerOffset;

    private Rigidbody rigidBody;
    private InputService inputService;
    private PlayerMovement playerPhysicsMovement;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerPhysicsMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Start()
    {
        inputService = ServiceLocator.Instance.Get<InputService>();

        if (jumpToDisconnect)
        {
            inputService.playerInputActions.PlayerActionMap.Jump.performed += DisconnectRaftInput;
        }
    }

    private void OnDisable()
    {
        inputService.playerInputActions.PlayerActionMap.Jump.performed -= DisconnectRaftInput;
    }

    private void Update()
    {
        if (!controllingRaft) { return; }

        inputDirection = inputService.playerInputActions.PlayerActionMap.Walk.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Movement();
        SpeedControl();
    }

    private void Movement()
    {
        if (!controllingRaft) { return; }

        moveDirection = cameraTransform.forward * inputDirection.y + cameraTransform.right * inputDirection.x;

        Vector3 moveForce = 10f * moveSpeed * moveDirection.normalized;

        rigidBody.AddForce(moveForce, ForceMode.Force);

        playerPhysicsMovement.rigidBody.velocity = rigidBody.velocity;
        playerPhysicsMovement.rigidBody.position = rigidBody.position + playerOffset;
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

    private void DisconnectRaftInput(InputAction.CallbackContext context)
    {
        DisconnectRaft();
    }

    public void ToggleRaftConnection()
    {
        if (controllingRaft) { DisconnectRaft(); }
        else { ConnectRaft(); }
    }

    public void DisconnectRaft()
    {
        controllingRaft = false;
        playerPhysicsMovement.SetCanMove(true);
        rigidBody.isKinematic = true;
    }

    public void ConnectRaft()
    {
        controllingRaft = true;
        playerPhysicsMovement.SetCanMove(false);
        rigidBody.isKinematic = false;

        playerOffset = playerPhysicsMovement.transform.position - transform.position;
    }
}

