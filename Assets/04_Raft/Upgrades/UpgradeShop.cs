using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeShop : MonoBehaviour
{
    [SerializeField] private List<Transform> upgradeLocations;
    [SerializeField] private List<RaftUpgrade> upgrades;
    [SerializeField] private List<TextMeshPro> signTxts;

    /// <summary>
    /// Stores the generated pickups and their corresponding health worths.
    /// </summary>
    private readonly Dictionary<GameObject, int> pickUpsHealth = new();

    private PlayerRaftUpgradeHolder upgradeHolder;
    private HealthDispenser healthDispenser;

    private void Start()
    {
        upgradeHolder = FindFirstObjectByType<PlayerRaftUpgradeHolder>();
        healthDispenser = FindFirstObjectByType<HealthDispenser>();
    }

    public void GeneratePickUps()
    {
        for (int i = 0; i < upgradeLocations.Count; i++)
        {
            int upgradeType = Random.Range(0, upgradeLocations.Count);
            RaftUpgrade upgrade = upgrades[upgradeType];

            GameObject pickUp = Instantiate(upgrade.pickUpPrefab, upgradeLocations[i].position, Quaternion.identity);
            pickUpsHealth.Add(pickUp, upgrade.healthWorth);

            signTxts[i].text = $"HP: {upgrade.healthWorth}";

            // assign callbacks for when one of the pickups is chosen
            InteractableButton interactableButton = pickUp.GetComponent<InteractableButton>();
            interactableButton.OnActivated.RemoveAllListeners();
            interactableButton.OnActivated.AddListener(() => { upgradeHolder.SetRaftUpgrade(upgrade); 
                PickUpChosen(pickUp); });
        }
    }

    public void PickUpChosen(GameObject chosenPickUp)
    {
        foreach (GameObject pickUp in pickUpsHealth.Keys)
        {
            // the pick ups that aren't chosen will be scrapped into health points
            // the amount of health points is related to the pick up type
            if (pickUp != chosenPickUp)
            {
                healthDispenser.AddHealthPoints(pickUpsHealth[pickUp]);
            }
           
            pickUp.SetActive(false);
        }

        foreach (TextMeshPro signTxt in signTxts)
        {
            signTxt.text = string.Empty;
        }
    }
}
