using UnityEngine;

/* Base of the PlayerController is from https://sharpcoderblog.com/blog/unity-3d-fps-controller.
 * Edited for use in this project. */

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool canMove = true;
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float runningSpeed = 11.5f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    public void SetCanMove(bool value) => canMove = value;

    public bool IsMoving { private set; get; } = false;
    public bool IsSprinting { private set; get; } = false;

    private InputService inputService;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputService = ServiceLocator.Instance.Get<InputService>();
    }

    private void Update()
    {
        HorizontalMovement();
    }

    private void HorizontalMovement()
    {
        if (!canMove) { return; }

        Vector2 inputDirection = inputService.playerInputActions.PlayerActionMap.Walk.ReadValue<Vector2>();
        
        if (inputDirection != Vector2.zero) { IsMoving = true; }
        else { IsMoving = false; }

        IsSprinting = inputService.playerInputActions.PlayerActionMap.Sprint.IsPressed();
        float forwardSpeed = (IsSprinting ? runningSpeed : walkingSpeed) * inputDirection.y;
        float rightSpeed = (IsSprinting ? runningSpeed : walkingSpeed) * inputDirection.x;
        float movementDirectionY = moveDirection.y;
        moveDirection = (transform.forward * forwardSpeed) + (transform.right * rightSpeed);

        if (inputService.playerInputActions.PlayerActionMap.Jump.WasPressedThisFrame() && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void WarpPlayer(Vector3 warpLocation)
    {
        transform.position = warpLocation;
        Physics.SyncTransforms();
    }
}