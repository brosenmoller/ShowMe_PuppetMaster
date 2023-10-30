using UnityEngine;

public class PlayerRaftUpgradeHolder : MonoBehaviour
{
    public RaftUpgrade currentHeldRaftUpgrade = null;

    public void SetRaftUpgrade(RaftUpgrade raftUpgrade)
    {
        currentHeldRaftUpgrade = raftUpgrade;
    }
}

