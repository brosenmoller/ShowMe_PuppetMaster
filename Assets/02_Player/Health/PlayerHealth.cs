using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageAble
{
    public int maxHealth = 4;
    [HideInInspector] public int health;
    [SerializeField] private Transform respawnPoint;

    private PlayerHealthUI healthUI;

    private void Awake()
    {
        healthUI = FindObjectOfType<PlayerHealthUI>();
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthUI.LooseHeart(damage);

        if (health <= 0)
        {
            Respawn();
        }
    }

    public void Revive(int amount)
    {
        health += amount;
        if (health > maxHealth) { health = maxHealth; }

        healthUI.ReviveHeart(amount);
    }

    public void AddAdditionalHearts(int amount)
    {
        healthUI.AddAddionalHearts(amount);
    }

    public void DamageAndRespawn()
    {
        TakeDamage(1);
        Respawn();
    }

    public void Respawn()
    {
        transform.position = respawnPoint.position;
    }
}
