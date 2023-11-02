using TMPro;
using UnityEngine;

public class HealthDispenser : MonoBehaviour
{
    [SerializeField] private int startingHealthPoints;
    [SerializeField] private TextMeshProUGUI healthPointsText;

    private int currentHealthPoints;
    public int CurrentHealthPoints
    {
        get { return currentHealthPoints; }
        set
        {
            currentHealthPoints = value; healthPointsText.text = currentHealthPoints.ToString();
        }
    }

    private PlayerHealth playerHealth;

    private void Start()
    {
        CurrentHealthPoints = startingHealthPoints;
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void GiveHealthToPlayer()
    {
        if (playerHealth.health >= playerHealth.maxHealth) { return; }
        if (CurrentHealthPoints < 1) { return; }

        CurrentHealthPoints--;
        playerHealth.Revive(1);
    }

    public void AddHealthPoints(int amount)
    {
        CurrentHealthPoints += amount;
    }
}
