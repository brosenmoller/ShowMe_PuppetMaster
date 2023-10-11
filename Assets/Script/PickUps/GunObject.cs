using UnityEngine;

public class GunObject : PickUpInteractable
{
	public GunScriptableObject gunData;

	private MeshFilter gunMesh;

	private void OnValidate()
	{
		gunMesh = GetComponent<MeshFilter>();
		UpdateGun();
	}

	private void Awake()
	{
		gunMesh = GetComponent<MeshFilter>();
	}

	private void OnEnable()
	{
		UpdateGun();
	}

	private void UpdateGun()
	{
		//gunMesh.mesh = gunData.gunMesh;
	}

	public GunScriptableObject TakeItem()
	{
		GunScriptableObject result = gunData;
		base.gameObject.SetActive(value: false);
		return result;
	}

	public GunScriptableObject ReplaceItem(GunScriptableObject currentGun)
	{
		if (currentGun == null)
		{
			base.gameObject.SetActive(value: false);
			return null;
		}
		GunScriptableObject result = gunData;
		gunData = currentGun;
		UpdateGun();
		return result;
	}
}
