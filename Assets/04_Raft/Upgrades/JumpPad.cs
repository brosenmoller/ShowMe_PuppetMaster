using UnityEngine;

[RequireComponent(typeof(Collider))]
public class JumpPad : MonoBehaviour
{
    [Header("JumpPad Settings")]
    [SerializeField] private float launchForce;
    [SerializeField] private float verticalComponent;

    private Collider objectCollider;

    private void Awake()
    {
        objectCollider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(objectCollider.bounds.center, objectCollider.bounds.extents, Quaternion.identity);

        foreach (Collider collider in colliders)
        {
            collider.TryGetComponent(out PlayerMovement playerMovement);
            if (playerMovement != null)
            {
                Launch(playerMovement);
            }
        }
    }

    private void Launch(PlayerMovement playerMovement)
    {
        playerMovement.rigidBody.AddForce(new Vector3(transform.forward.x, 1.2f, transform.forward.z) * launchForce, ForceMode.Impulse);
    }
}
