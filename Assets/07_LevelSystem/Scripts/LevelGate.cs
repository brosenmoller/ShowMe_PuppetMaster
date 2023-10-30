using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LevelGate : MonoBehaviour
{
    [SerializeField] private TextMeshPro enemiesLeftTxt;
    [SerializeField] private List<Target> enemies;

    private void Start()
    {
        foreach (Target enemy in enemies)
        {
            enemy.OnDeath.AddListener(UpdateEnemyCount);
        }
        UpdateEnemyCount();
    }

    private void UpdateEnemyCount()
    {
        int remainingEnemies = enemies.Count(x => x != null && x.IsAlive);
        enemiesLeftTxt.text = $"Enemies: {remainingEnemies} / {enemies.Count()}";

        if (remainingEnemies == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
