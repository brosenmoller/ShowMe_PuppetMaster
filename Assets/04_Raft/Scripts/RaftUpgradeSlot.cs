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

    public override bool CheckIfLocked()
    {
        return upgradeHolder.currentHeldRaftUpgrade == null || currentRaftUpgarde == null;
    }

    protected override void PerformInteraction()
    {
        if (currentRaftUpgarde != null)
        {
            Destroy(spawnedObject);
            upgradeHolder.currentHeldRaftUpgrade = currentRaftUpgarde;
            currentRaftUpgarde = null;
            return;
        }

        if (upgradeHolder.currentHeldRaftUpgrade != null) 
        {
            currentRaftUpgarde = upgradeHolder.currentHeldRaftUpgrade;
            upgradeHolder.currentHeldRaftUpgrade = null;
            spawnedObject = Instantiate(currentRaftUpgarde.prefab, transform);
            spawnedObject.transform.localPosition = Vector3.zero;
        }
    }
}
