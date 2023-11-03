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
        if (currentRaftUpgarde != null && upgradeHolder.CurrentHeldRaftUpgrade == null)
        {
            interactionDescription = $"Pick up {currentRaftUpgarde.upgradeName}";
        }
        else if (currentRaftUpgarde == null && upgradeHolder.CurrentHeldRaftUpgrade != null)
        {
            interactionDescription = $"Place {upgradeHolder.CurrentHeldRaftUpgrade.upgradeName}";
        }
        else
        {
            interactionDescription = "";
        }
    }

    protected override void PerformInteraction()
    {
        if (currentRaftUpgarde != null && upgradeHolder.CurrentHeldRaftUpgrade == null)
        {
            Destroy(spawnedObject);
            upgradeHolder.SetRaftUpgrade(currentRaftUpgarde);
            currentRaftUpgarde = null;
            return;
        }

        if (currentRaftUpgarde == null && upgradeHolder.CurrentHeldRaftUpgrade != null) 
        {
            currentRaftUpgarde = upgradeHolder.CurrentHeldRaftUpgrade;
            upgradeHolder.SetRaftUpgrade(null);
            spawnedObject = Instantiate(currentRaftUpgarde.prefab, transform);
            spawnedObject.transform.localPosition = currentRaftUpgarde.spawnOffset;
        }
    }
}
