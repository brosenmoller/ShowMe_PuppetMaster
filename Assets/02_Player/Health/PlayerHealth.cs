using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageAble
{
    public int maxHealth = 4;
    [HideInInspector] public int health;

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}
