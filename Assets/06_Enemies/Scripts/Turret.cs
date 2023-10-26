using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] private float shootInterval = 5.0f;
    [SerializeField] private float shootIntervalDeviation = 0.5f;
    [SerializeField] private float bulletSpeed = 10.0f;

    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private Vector3 direction;
    private Quaternion lookRotation;
    private Quaternion lookRotationOnlyY;
    private float timeUntilShoot;

    private void Start()
    {
        StartCoroutine(Shooting());
    }

    private void FixedUpdate()
    {
        LookTowardsTarget();
    }

    private void LookTowardsTarget()
    {
        direction = (target.position - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
        lookRotationOnlyY = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = lookRotationOnlyY;
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            timeUntilShoot = Random.Range(shootInterval - shootIntervalDeviation, shootInterval + shootIntervalDeviation);

            yield return new WaitForSeconds(timeUntilShoot);

            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, lookRotation);
        bullet.GetComponent<Bullet>().Setup(bulletSpeed);
    }
}
