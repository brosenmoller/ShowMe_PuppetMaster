using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("JumpPad Settings")]
    [SerializeField] private float launchForce;
    [SerializeField] private float verticalComponent;

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent(out PlayerMovement player);
        if (player != null)
        {
            player.rigidBody.AddForce(new Vector3 (transform.forward.x, 1.2f, transform.forward.z) * launchForce, ForceMode.Impulse);
        }
    }
}
