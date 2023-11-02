using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float lifeTime;

    private float bulletSpeed;

    public void Setup(float bulletSpeed)
    {
        this.bulletSpeed = bulletSpeed;
    }

    private void Start()
    {
        CheckIfSpawnedInsideCollider();
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        CheckForCollisions();
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
    }

    private void CheckForCollisions()
    {
        if (Physics.SphereCast(transform.position, radius, transform.forward, out RaycastHit hit, bulletSpeed * Time.deltaTime, layerMask, QueryTriggerInteraction.Collide))
        {
            AffectHit(hit.transform);
        }
    }

    private void CheckIfSpawnedInsideCollider()
    {
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 0.1f, hitColliders, layerMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < numColliders; i++)
        {
            AffectHit(hitColliders[i].transform);
        }
    }

    protected virtual void AffectHit(Transform hit)
    {
        hit.TryGetComponent(out IDamageAble damageAble);
        damageAble?.TakeDamage(damage);

        Destroy(gameObject);
    }
}
