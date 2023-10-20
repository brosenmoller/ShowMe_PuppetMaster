using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private int damage;

    private float bulletSpeed;

    public void Setup(float bulletSpeed)
    {
        this.bulletSpeed = bulletSpeed;
    }

    private void Update()
    {
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out IDamageAble damageAble);
        damageAble?.TakeDamage(damage);

        Destroy(gameObject);
    }
}
