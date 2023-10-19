using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private int fireRate = 1;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;

    [Header("References")]
    [SerializeField] private Transform bulletSpawnPoint;

    private InputService inputService;

    private void Start()
    {
        inputService = ServiceLocator.Instance.Get<InputService>();

        inputService.playerInputActions.PlayerActionMap.Shoot.performed += Shoot;
    }

    private void OnDisable()
    {
        inputService.playerInputActions.PlayerActionMap.Shoot.performed -= Shoot;
    }

    private void Shoot(InputAction.CallbackContext callbackContext)
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        newBullet.GetComponent<Bullet>().Setup(bulletSpeed);
    }
}
