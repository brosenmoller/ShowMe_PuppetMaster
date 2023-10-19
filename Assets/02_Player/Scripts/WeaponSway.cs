using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smoothing;
    [SerializeField] private float swayMultiplier;

    private InputService inputService;

    private void Start()
    {
        inputService = ServiceLocator.Instance.Get<InputService>();
    }

    private void Update()
    {
        float mouseX = inputService.playerInputActions.PlayerActionMap.MoveCameraX.ReadValue<float>() * swayMultiplier;
        float mouseY = inputService.playerInputActions.PlayerActionMap.MoveCameraY.ReadValue<float>() * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothing * Time.deltaTime);
    }
}
