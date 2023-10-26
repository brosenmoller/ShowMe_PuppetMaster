using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageAble
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Material whiteFlashMaterial;

    private int health;
    private Material defaultMaterial;
    private MeshRenderer meshRenderer;

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
            Destroy(gameObject);
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
}
