using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    private readonly string looseHeart = "LooseHeart";
    private readonly string reviveHeart = "ReviveHeart";

    [Header("Heart Object")]
    [SerializeField] private GameObject heartPrefab;

    [Header("Heart Instantiate")]
    [SerializeField] private float timeBetweenInstantiates;

    private readonly List<GameObject> currentFullHearts = new();
    private readonly List<GameObject> currentEmptyHearts = new();

    private PlayerHealth player;

    private void Start()
    {
        player = FindObjectOfType<PlayerHealth>();  
        InstantiateHeart(player.maxHealth);
        LooseHeart(player.maxHealth - player.health);
    }

    public void InstantiateHeart(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            heart.name = "Heart " + (i + 1).ToString();

            currentFullHearts.Add(heart);
        }
    }

    public void LooseHeart(int damage)
    {
        if (damage > currentFullHearts.Count) damage = currentFullHearts.Count;
        if (damage <= 0) return;

        StartCoroutine(LoosingHearts(damage));
    }

    private IEnumerator LoosingHearts(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            int index = currentFullHearts.Count - 1;

            currentFullHearts[index].GetComponent<Animator>().Play(looseHeart);
            currentEmptyHearts.Insert(0, currentFullHearts[index]);
            currentFullHearts.RemoveAt(index);

            yield return new WaitForSeconds(timeBetweenInstantiates);
        }
    }

    public void ReviveHeart(int amount)
    {
        if (amount > currentEmptyHearts.Count) amount = currentEmptyHearts.Count;
        if (amount <= 0) return;

        StartCoroutine(RevivingHearts(amount));
    }

    private IEnumerator RevivingHearts(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (currentEmptyHearts.Count == 0) break;
            currentEmptyHearts[0].GetComponent<Animator>().Play(reviveHeart);
            currentFullHearts.Add(currentEmptyHearts[0]);
            currentEmptyHearts.RemoveAt(0);

            yield return new WaitForSeconds(timeBetweenInstantiates);
        }
    }

    public void AddAddionalHearts(int amount)
    {
        StartCoroutine(AddingAdditionalHearts(amount));
    }

    private IEnumerator AddingAdditionalHearts(int amount)
    {
        int reviveAmount = player.maxHealth;
        if (reviveAmount > currentEmptyHearts.Count) reviveAmount = currentEmptyHearts.Count;
        if (reviveAmount > 0)
        {
            yield return RevivingHearts(reviveAmount);
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            heart.name = "Heart " + (i + 1 + player.maxHealth).ToString();

            currentFullHearts.Add(heart);

            yield return new WaitForSeconds(timeBetweenInstantiates);
        }

        player.maxHealth += amount;
        player.health = player.maxHealth;
    }
}
