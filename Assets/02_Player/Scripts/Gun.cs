using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private int fireRate = 1;
    [SerializeField] private float bulletSpeed;

    [Header("Gun Settings")]
    [SerializeField] private bool canAim;
    [SerializeField] private float aimAnimationDurattion;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private InputService inputService;

    private Vector3 defatultPosition;
    private Vector3 aimPosition;

    private void Start()
    {
        inputService = ServiceLocator.Instance.Get<InputService>();

        inputService.playerInputActions.PlayerActionMap.Shoot.performed += Shoot;


        if (canAim)
        {
            defatultPosition = transform.localPosition;
            aimPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            inputService.playerInputActions.PlayerActionMap.Aim.started += AimStart;
            inputService.playerInputActions.PlayerActionMap.Aim.canceled += AimCancel;
        }
    }

    private void OnDisable()
    {
        inputService.playerInputActions.PlayerActionMap.Shoot.performed -= Shoot;
        if (canAim)
        {
            inputService.playerInputActions.PlayerActionMap.Aim.started -= AimStart;
            inputService.playerInputActions.PlayerActionMap.Aim.canceled -= AimCancel;
        }
    }

    private void Shoot(InputAction.CallbackContext callbackContext)
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        newBullet.GetComponent<Bullet>().Setup(bulletSpeed);
    }

    private void AimCancel(InputAction.CallbackContext callbackContext)
    {
        StopAllCoroutines();
        StartCoroutine(MoveGun(defatultPosition));
    }

    private void AimStart(InputAction.CallbackContext callbackContext)
    {
        StopAllCoroutines();
        StartCoroutine(MoveGun(aimPosition));
    }

    private IEnumerator MoveGun(Vector3 targetPosition)
    {
        Vector3 fromPosition = transform.localPosition;

        float time = 0f;
        while (time <= aimAnimationDurattion)
        {
            transform.localPosition = Vector3.Lerp(fromPosition, targetPosition, 1 - Mathf.Pow(1 - time / aimAnimationDurattion, 2));
            time += Time.deltaTime;
            yield return null;
        }
    }
}
