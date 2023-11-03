using UnityEngine;

public class PlayerRaftUpgradeHolder : MonoBehaviour
{
    public RaftUpgrade CurrentHeldRaftUpgrade { get; private set; } = null;
    [SerializeField] private Collider[] raftUpgradeSlotColliders;

    private void Awake()
    {
        DisableAllUpgradeSlotColliders();
    }

    public void SetRaftUpgrade(RaftUpgrade raftUpgrade)
    {
        CurrentHeldRaftUpgrade = raftUpgrade;
        Debug.Log(raftUpgrade);

        if (CurrentHeldRaftUpgrade != null)
        {
            EnableAllUpgradeSlotColliders();
        }
        else
        {
            DisableAllUpgradeSlotColliders();
        }
    }

    private void DisableAllUpgradeSlotColliders()
    {
        foreach (Collider collider in raftUpgradeSlotColliders)
        {
            collider.enabled = false;
        }
    }

    private void EnableAllUpgradeSlotColliders()
    {
        foreach (Collider collider in raftUpgradeSlotColliders)
        {
            collider.enabled = true;
        }
    }
}

