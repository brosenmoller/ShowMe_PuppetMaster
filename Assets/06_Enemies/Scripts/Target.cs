using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class Target : MonoBehaviour, IDamageAble
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Material whiteFlashMaterial;

    private int health;
    private Material defaultMaterial;
    private MeshRenderer meshRenderer;

    public bool IsAlive { get { return health > 0; } }
    public UnityEvent OnDeath;

    private void Awake()
    {
        health = maxHealth;
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = meshRenderer.material;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            OnDeath.Invoke();
            Destroy(gameObject, .2f);
        }
        else
        {
            CancelInvoke(nameof(ResetMaterial));
            meshRenderer.material = whiteFlashMaterial;
            Invoke(nameof(ResetMaterial), flashDuration);
        }
    }

    private void ResetMaterial()
    {
        meshRenderer.material = defaultMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathTrigger"))
        {
            OnDeath.Invoke();
            Destroy(gameObject, .2f);
        }
    }
}
