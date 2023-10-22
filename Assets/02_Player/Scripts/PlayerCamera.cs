using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform player;
    [SerializeField] private float lookSpeed = 0.2f;
    [SerializeField] private float lookXLimit = 85.0f;

    private float rotationX = 0;
    private InputService inputService;

    private void Start()
    {
        inputService = ServiceLocator.Instance.Get<InputService>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = player.position;
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        rotationX += -inputService.playerInputActions.PlayerActionMap.MoveCameraY.ReadValue<float>() * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, inputService.playerInputActions.PlayerActionMap.MoveCameraX.ReadValue<float>() * lookSpeed, 0);
    }
}
