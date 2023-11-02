using UnityEngine;

public class HarpoonBullet : Bullet
{
    [Header("Pull Settings")]
    [SerializeField] private float pullStrength = 100f;
    [SerializeField] private float pullUpStrength = 10f;

    protected override void AffectHit(Transform hit)
    {
        hit.TryGetComponent(out Target target);
        if (target != null)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(pullStrength * -1f * transform.forward + Vector3.up * pullUpStrength, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}

