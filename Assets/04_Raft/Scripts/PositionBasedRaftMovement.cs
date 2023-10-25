using UnityEngine;

public class PositionBasedRaftMovement : MonoBehaviour
{
    [Header("Raft Settings")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("References")]
    [SerializeField] private Collider forwardButton;
    [SerializeField] private Collider backButton;
    [SerializeField] private Collider rightButton; 
    [SerializeField] private Collider leftButton; 

    private PlayerMovement player;
    private Rigidbody rigidBody;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        SpeedControl();
        MoveRaft();
    }

    private void MoveRaft()
    {
        if (!player.IsGrounded) { return; }

        bool collided = true;

        if (forwardButton.bounds.Contains(player.transform.position))
        {
            rigidBody.AddForce(10f * moveSpeed * Vector3.forward);
        }
        else if (backButton.bounds.Contains(player.transform.position))
        {
            rigidBody.AddForce(10f * moveSpeed * Vector3.back);
        }
        else if (rightButton.bounds.Contains(player.transform.position))
        {
            rigidBody.AddForce(10f * moveSpeed * Vector3.right);
        }
        else if (leftButton.bounds.Contains(player.transform.position))
        {
            rigidBody.AddForce(10f * moveSpeed * Vector3.left);
        }
        else
        {
            collided = false;
        }

        if (collided)
        {
            player.rigidBody.AddForce(rigidBody.velocity);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rigidBody.velocity = new Vector3(limitedVelocity.x, rigidBody.velocity.y, limitedVelocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent(out PlayerMovement playerPhysicsMovement);
        if (playerPhysicsMovement != null)
        {
            playerPhysicsMovement.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.TryGetComponent(out PlayerMovement playerPhysicsMovement);
        if (playerPhysicsMovement != null)
        {
            playerPhysicsMovement.transform.parent = null;
        }
    }
}
