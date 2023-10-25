using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    private PlayerPhysicsMovement player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [SerializeField] private float shootInterval = 5.0f;
    [SerializeField] private float shootIntervalDeviation = 0.5f;
    [SerializeField] private float bulletSpeed = 10.0f;

    private Vector3 direction;
    private Quaternion lookRotation;
    private Quaternion lookRotationOnlyY;
    private float timeUntilShoot;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerPhysicsMovement>();
    }

    private void Start()
    {
        StartCoroutine(Shooting());
    }

    private void FixedUpdate()
    {
        LookTowardsPlayer();
    }

    private void LookTowardsPlayer()
    {
        direction = (player.transform.position - transform.position).normalized;
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
