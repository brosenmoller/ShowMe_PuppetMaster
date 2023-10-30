using UnityEngine;

public class PlayerRaftUpgradeHolder : MonoBehaviour
{
    public RaftUpgrade currentHeldRaftUpgrade;

    public void SetRaftUpgrade(RaftUpgrade raftUpgrade)
    {
        currentHeldRaftUpgrade = raftUpgrade;
    }
}

