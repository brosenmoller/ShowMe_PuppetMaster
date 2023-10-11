using UnityEngine;

public class UpgradePoint : MonoBehaviour
{
	public int upgradePointID;

	private RaftUpgradePickUp currentUpgrade;

	[SerializeField]
	//private RaftUpgradeLifter raftUpgradeLifter;

	public bool CanUpgrade(RaftUpgradePickUp upgrade)
	{
		if (currentUpgrade != null)
		{
			return false;
		}
		upgrade.transform.parent = base.transform;
		upgrade.transform.localPosition = Vector3.zero;
		upgrade.transform.localRotation = Quaternion.identity;
		upgrade.gameObject.SetActive(value: true);
		upgrade.Place();
		currentUpgrade = upgrade;
		/*if (upgrade.liftUpgrade)
		{
			raftUpgradeLifter.offset = -45f;
		}
		else
		{
			raftUpgradeLifter.offset = 0f;
		}*/
		return true;
	}

	public bool CanPlaceUpgrade()
	{
		if (currentUpgrade != null)
		{
			return false;
		}
		return true;
	}

	public RaftUpgradePickUp TakeUpgrade()
	{
		RaftUpgradePickUp result = currentUpgrade;
		Debug.Log("removed " + currentUpgrade.name);
		currentUpgrade = null;
		//raftUpgradeLifter.offset = -45f;
		return result;
	}

	public RaftUpgradePickUp GetUpgrade()
	{
		return currentUpgrade;
	}
}
