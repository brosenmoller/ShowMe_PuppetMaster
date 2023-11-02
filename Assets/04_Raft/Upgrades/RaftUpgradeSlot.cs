using UnityEngine;

public class RaftUpgradeSlot : InteractableObject
{
    private PlayerRaftUpgradeHolder upgradeHolder;
    private RaftUpgrade currentRaftUpgarde = null;

    private GameObject spawnedObject;

    private void Start()
    {
        upgradeHolder = FindObjectOfType<PlayerRaftUpgradeHolder>();
    }

    private void FixedUpdate()
    {
        if (currentRaftUpgarde != null && upgradeHolder.currentHeldRaftUpgrade == null)
        {
            interactionDescription = $"Pick up {currentRaftUpgarde.upgradeName}";
        }
        else if (currentRaftUpgarde == null && upgradeHolder.currentHeldRaftUpgrade != null)
        {
            interactionDescription = $"Place {upgradeHolder.currentHeldRaftUpgrade.upgradeName}";
        }
        else
        {
            interactionDescription = "";
        }
    }

    protected override void PerformInteraction()
    {
        if (currentRaftUpgarde != null && upgradeHolder.currentHeldRaftUpgrade == null)
        {
            Destroy(spawnedObject);
            upgradeHolder.currentHeldRaftUpgrade = currentRaftUpgarde;
            currentRaftUpgarde = null;
            return;
        }

        if (currentRaftUpgarde == null && upgradeHolder.currentHeldRaftUpgrade != null) 
        {
            currentRaftUpgarde = upgradeHolder.currentHeldRaftUpgrade;
            upgradeHolder.currentHeldRaftUpgrade = null;
            spawnedObject = Instantiate(currentRaftUpgarde.prefab, transform);
            spawnedObject.transform.localPosition = currentRaftUpgarde.spawnOffset;
        }
    }
}
