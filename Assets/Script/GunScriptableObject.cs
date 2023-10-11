using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun", order = 1)]
public class GunScriptableObject : ScriptableObject
{
	public Mesh gunMesh;

	public Mesh magazineMesh;

	[Space]
	public Sprite weaponArt;

	public float DPS;

	[Space]
	public float roundsPerSecond;

	public float roundsInterval;

	[Space]
	public float bulletDamage = 10f;

	[Space]
	public int magazineSize;

	public float reloadDuration;

	[Space]
	public Vector2 bulletSpread = new Vector2(2f, 2f);

	public float knockbackMultiplier;

	[Space]
	public float bulletSizeMultiplier = 1f;

	public float bulletSpeed = 30f;

	public int bulletsAtOnce = 1;

	public bool fullAuto;

	public bool burst;

	public int burstSize;

	public float burstRoundsPerSecond;

	public float burstInterval;

	public float minPitch;

	public float maxPitch;

	[SerializeField]
	private List<AudioClip> audioClips_Shoot;

	public AudioClip audioClip_Reload1;

	public AudioClip audioClip_Reload2;

	public Vector3 barrelOffset;

	public GameObject _muzzleFXFlashPrefab;

	public GameObject _muzzleFXSmokePrefab;

	public int animLayerIndex;

	public AudioClip AudioClipShoot => audioClips_Shoot[Random.Range(0, audioClips_Shoot.Count)];

	public float Pitch => Random.Range(minPitch, maxPitch);

	private void OnValidate()
	{
		burstInterval = 1f / burstRoundsPerSecond;
		roundsInterval = 1f / roundsPerSecond;
		float num = 1f;
		if (burst)
		{
			num = burstSize;
		}
		float num2 = bulletDamage * (float)bulletsAtOnce * num * (float)magazineSize;
		float num3 = (float)magazineSize * roundsInterval + reloadDuration;
		DPS = num2 / num3;
	}
}
