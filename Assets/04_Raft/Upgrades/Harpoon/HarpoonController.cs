using Cinemachine;
using UnityEngine;

public class HarpoonController : InteractableObject
{
    [Header("Camera Settings")]
    [SerializeField] private float lookSpeed = 0.3f;
    [SerializeField] private float lookXLimitUp = 45.0f;
    [SerializeField] private float lookXLimitDown = -10.0f;

    private bool controllingHarpoon = false;

    private float rotationX = 0;
    private InputService inputService;

    private CinemachineVirtualCamera harpoonCamera;
    private PlayerCamera playerCamera;
    private PlayerMovement playerMovement;
    private HarpoonGun harpoonGun;
    private Gun playerGun;

    private void Start()
    {
        harpoonCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        harpoonGun = FindObjectOfType<HarpoonGun>();
        playerGun = FindObjectOfType<Gun>();

        inputService = ServiceLocator.Instance.Get<InputService>();
    }

    private void Update()
    {
        if (controllingHarpoon && 
            (inputService.playerInputActions.PlayerActionMap.Jump.WasPressedThisFrame() || 
            inputService.playerInputActions.PlayerActionMap.Interact.WasPerformedThisFrame()))
        {
            controllingHarpoon = false;
            harpoonCamera.Priority = 0;
            playerCamera.CanMove = true;
            playerMovement.SetCanMove(true);
            harpoonGun.CanShoot = false;
            playerGun.CanShoot = true;
        }

        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (!controllingHarpoon) { return; }

        rotationX += -inputService.playerInputActions.PlayerActionMap.MoveCameraY.ReadValue<float>() * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, lookXLimitDown, lookXLimitUp);
        harpoonCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.localRotation *= Quaternion.Euler(0, inputService.playerInputActions.PlayerActionMap.MoveCameraX.ReadValue<float>() * lookSpeed, 0);
    }

    protected override void PerformInteraction()
    {
        harpoonCamera.Priority = 2;
        controllingHarpoon = true;
        playerCamera.CanMove = false;
        playerMovement.SetCanMove(false);
        harpoonGun.CanShoot = true;
        playerGun.CanShoot = false;
    }
}
