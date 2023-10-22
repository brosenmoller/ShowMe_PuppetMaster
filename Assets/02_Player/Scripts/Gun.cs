using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] private float shotDelay = 0.1f;
    [SerializeField] private float bulletSpeed = 200;
    [SerializeField] private int maxRoundSize = 6;
    [SerializeField] private float reloadTime = 0.3f;
    [SerializeField] private float defaultMaxRandomOffset = 0.1f;
    [SerializeField] private float aimMaxRandomOffset = 0.05f;
    [SerializeField] private float gunKnockBack = 5f;
    [SerializeField] private float gunKnockAirMultiplier = 2f;

    [Header("Gun Settings")]
    [SerializeField] private bool canAim = true;
    [SerializeField] private float aimAnimationDurattion = 0.2f;
    [SerializeField] private bool automaticReload = true;
    [SerializeField] private bool automaticFire = true;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    private Rigidbody playerRigidBody;
    private PlayerPhysicsMovement playerPhysics;

    [Header("Events")]
    [SerializeField] private UnityEvent OnReload;
    [SerializeField] private UnityEvent OnShoot;
    [SerializeField] private UnityEvent<int> OnCurrentRoundChange;

    private InputService inputService;

    private Vector3 defatultPosition;
    private Vector3 aimPosition;

    private int currentRoundSize;
    private bool isShooting;

    private void Start()
    {
        playerPhysics = FindObjectOfType<PlayerPhysicsMovement>();
        playerRigidBody = playerPhysics.gameObject.GetComponent<Rigidbody>();

        currentRoundSize = maxRoundSize;
        OnCurrentRoundChange?.Invoke(currentRoundSize);

        inputService = ServiceLocator.Instance.Get<InputService>();

        inputService.playerInputActions.PlayerActionMap.Shoot.started += StartShooting;
        inputService.playerInputActions.PlayerActionMap.Shoot.canceled += CancelShooting;
        inputService.playerInputActions.PlayerActionMap.Reload.performed += ManualReload;

        if (canAim)
        {
            defatultPosition = transform.localPosition;
            aimPosition = new Vector3(0, -0.5f, transform.localPosition.z);
            inputService.playerInputActions.PlayerActionMap.Aim.started += AimStart;
            inputService.playerInputActions.PlayerActionMap.Aim.canceled += AimCancel;
        }
    }

    private void OnDisable()
    {
        inputService.playerInputActions.PlayerActionMap.Shoot.started -= StartShooting;
        inputService.playerInputActions.PlayerActionMap.Shoot.canceled -= CancelShooting;
        inputService.playerInputActions.PlayerActionMap.Reload.performed -= ManualReload;

        if (canAim)
        {
            inputService.playerInputActions.PlayerActionMap.Aim.started -= AimStart;
            inputService.playerInputActions.PlayerActionMap.Aim.canceled -= AimCancel;
        }
    }

    private void StartShooting(InputAction.CallbackContext callbackContext)
    {
        isShooting = true;
        StartCoroutine(ShootCoroutine());
    }

    private void CancelShooting(InputAction.CallbackContext callbackContext)
    {
        StopAllCoroutines();
        isShooting = false;
    }

    private IEnumerator ShootCoroutine()
    {
        while (isShooting)
        {
            if (currentRoundSize <= 0)
            {
                if (automaticReload) { Invoke(nameof(Reload), reloadTime); }
                yield break;
            }

            currentRoundSize--;

            OnShoot?.Invoke();
            OnCurrentRoundChange?.Invoke(currentRoundSize);

            Vector3 knockBackForce = -1f * gunKnockBack * bulletSpawnPoint.forward;
            if (!playerPhysics.IsGrounded) { knockBackForce *= gunKnockAirMultiplier; }
            playerRigidBody.AddForce(knockBackForce, ForceMode.Impulse);

            GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            newBullet.GetComponent<Bullet>().Setup(bulletSpeed);

            if (!automaticFire) 
            {
                isShooting = false;
                yield break;
            }

            yield return new WaitForSeconds(shotDelay);
        }
    }

    private void ManualReload(InputAction.CallbackContext callbackContext)
    {
        Invoke(nameof(Reload), reloadTime);
    }

    private void Reload()
    {
        currentRoundSize = maxRoundSize;

        OnReload?.Invoke();
        OnCurrentRoundChange?.Invoke(currentRoundSize);
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
