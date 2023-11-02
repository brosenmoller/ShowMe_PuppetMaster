using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HarpoonGun : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] private float shotDelay = 0.1f;
    [SerializeField] private float bulletSpeed = 200;
    [SerializeField] private float maxRandomOffset = 3f;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("Events")]
    [SerializeField] private UnityEvent OnShoot;

    private InputService inputService;

    public bool CanShoot { get; set; } = false;

    private void Start()
    {
        inputService = ServiceLocator.Instance.Get<InputService>();

        inputService.playerInputActions.PlayerActionMap.Shoot.started += Shoot;
    }

    private void OnDisable()
    {
        inputService.playerInputActions.PlayerActionMap.Shoot.started -= Shoot;
    }

    private void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (!CanShoot) { return; }

        OnShoot?.Invoke();

        GameObject newBullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            GetRandomRotation(maxRandomOffset)
        );
        newBullet.GetComponent<Bullet>().Setup(bulletSpeed);
    }

    private Quaternion GetRandomRotation(float maxOffset)
    {
        return Quaternion.Euler(
            bulletSpawnPoint.rotation.eulerAngles +
            new Vector3(
                Random.Range(-maxOffset, maxOffset),
                Random.Range(-maxOffset, maxOffset),
                Random.Range(-maxOffset, maxOffset)
            )
        );
    }
}
