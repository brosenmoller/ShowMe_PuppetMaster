using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public PlayerController owner;
    public GameObject BulletPrefab;
	public Camera cam;
	[SerializeField]
	public GunScriptableObject primaryGunData;
    private Vector3 projectileStartVector;
	[SerializeField]
	private Transform projectileStart;
	private float timestamp;
	[SerializeField]
	private PhysicsRaft raft;
	[SerializeField]
	private LayerMask shouldMoveRaftLayer;

	[SerializeField]
	private LayerMask shouldBlockMoveRay;
	[SerializeField]
	private AnimationCurve curveRaftForce;

	[SerializeField]
	private float knockBackUp;
	[SerializeField]
	private float knockbackForce;

	[SerializeField]
	public int bulletsInMag;

	private int burstBulletsLeft;

	private bool isTriggerPresssed;

	private bool isReloading;

	private bool readyToShoot;
	private Coroutine reloadCoroutine;

	GunScriptableObject secondaryGunData;
	bool inSwap;
	int bulletsInSecondary;

	public Rigidbody rb;
	// Start is called before the first frame update
	void Start()
    {
		raft = FindObjectOfType<PhysicsRaft>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void SpawnAndSetBullet()
	{
		Quaternion rotation = cam.transform.rotation;
		Vector2 vector = new Vector2(Random.Range(0f - primaryGunData.bulletSpread.x, primaryGunData.bulletSpread.x), Random.Range(0f - primaryGunData.bulletSpread.y, primaryGunData.bulletSpread.y));
		if (vector.magnitude > (primaryGunData.bulletSpread.x + primaryGunData.bulletSpread.y) / 2f)
		{
			vector = vector * ((primaryGunData.bulletSpread.x + primaryGunData.bulletSpread.y) / 2f) / vector.magnitude;
		}
		rotation *= Quaternion.Euler(new Vector3(vector.y, vector.x, 0f));
		Vector3 vector2 = projectileStart.position + projectileStart.transform.forward;
		int num = Random.Range(1, 100);
		PlaceBulletInWorld(vector2, rotation, num, 0);
	}

	public void PlaceBulletInWorld(Vector3 projectilePos, Quaternion targetRot, int rngChance, int id)
	{
		Instantiate(BulletPrefab, projectilePos, targetRot);
	}

	private IEnumerator ReloadGun()
	{
		//gunSounds.PlaySoundReload1(primaryGunData);
		//armsAnim.speed = 1f + (1f - primaryGunData.reloadDuration / primaryGunData.reloadDuration);
		/*if (!armsAnim.GetCurrentAnimatorStateInfo(0).IsName("arm_idle_01"))
		{
			armsAnim.Play("arm_reload_01", primaryGunData.animLayerIndex);
			armsAnim.Update(0f);
		}
		else
		{
			armsAnim.SetTrigger("Reload");
		}*/
		isReloading = true;
		//bulletsText.text = "...";
		yield return new WaitForSeconds(1);
		//armsAnim.speed = 1f;
		//gunSounds.PlaySoundReload2(primaryGunData);
		bulletsInMag = primaryGunData.magazineSize;
		UpdateBullets();
		isReloading = false;
	}

	private void Shoot()
	{
		for (int i = 0; i < primaryGunData.bulletsAtOnce; i++)
		{
			SpawnAndSetBullet();
		}
		//gunSounds.PlaySoundShoot(primaryGunData);
		timestamp = Time.time;
		float num = 0f;
		if (raft != null && raft.IsPlayerOnRaft)
		{
			Vector3 direction = cam.transform.rotation * Vector3.forward;
			Vector2 vector = new Vector2(direction.x, direction.z);
			vector.Normalize();
			Vector2 vector2 = vector * -5f;
			float num2 = 15f;
			if (Physics.Raycast(cam.transform.position, direction, out var hitInfo, num2, (int)shouldMoveRaftLayer + (int)shouldBlockMoveRay, QueryTriggerInteraction.Ignore) && (shouldBlockMoveRay.value & (1 << hitInfo.transform.gameObject.layer)) == 0)
			{
				num = 1f - Mathf.Clamp01(hitInfo.distance / num2);
				num = curveRaftForce.Evaluate(num);
				vector2 *= num;
				raft.ShootToAddForce(vector2.x, vector2.y);
				if (raft.IsRaftBlocked)
				{
					num = 0f;
				}
			}
		}
		rb.AddForce(Vector3.up * knockBackUp * primaryGunData.knockbackMultiplier * (1f - num));
		rb.AddForce(-cam.transform.forward * knockbackForce * primaryGunData.knockbackMultiplier * (1f - num));
		bulletsInMag--;
		UpdateBullets();
		if (bulletsInMag <= 0 && !isReloading)
		{
			StartReload();
		}
	}
	private void StartReload()
	{
		if (reloadCoroutine != null)
		{
			StopCoroutine(reloadCoroutine);
		}
		reloadCoroutine = StartCoroutine(ReloadGun());
	}

	private void UpdateBullets()
	{
		//bulletsText.text = bulletsInMag.ToString();
	}
	public void DoReloadGun()
	{
		if (!isReloading && bulletsInMag < primaryGunData.magazineSize)
		{
			StartReload();
		}
	}

	public void DoTriggerPulled()
	{
		isTriggerPresssed = true;
		CheckShoot();
	}
	private void CheckShoot()
	{
		if (timestamp + 1f / (primaryGunData.roundsPerSecond) <= Time.time && bulletsInMag > 0 && !isReloading)
		{
			if (primaryGunData.fullAuto)
			{
				StartCoroutine(FullAuto());
			}
			else if (primaryGunData.burst)
			{
				StartCoroutine(BurstFire());
			}
			else
			{
				Shoot();
			}
		}
        else
        {
			if(timestamp + 1f / (primaryGunData.roundsPerSecond) <= Time.time && bulletsInMag == 0 && !isReloading)
            {
				StartReload();
            }
        }
	}

	private IEnumerator FullAuto()
	{
		while (isTriggerPresssed)
		{
			if (bulletsInMag > 0 && !isReloading)
			{
				if (primaryGunData.burst)
				{
					StartCoroutine(BurstFire());
				}
				else
				{
					Shoot();
				}
			}
			yield return new WaitForSeconds(1f / primaryGunData.roundsPerSecond);
		}
	}

	private IEnumerator BurstFire()
	{
		burstBulletsLeft = primaryGunData.burstSize;
		while (burstBulletsLeft > 0 && bulletsInMag > 0 && !isReloading)
		{
			Shoot();
			burstBulletsLeft--;
			yield return new WaitForSeconds(1f / primaryGunData.roundsPerSecond);
		}
	}
	public void DoTriggerReleased()
	{
		isTriggerPresssed = false;
	}

	public void GunSwapped()
	{
		if(secondaryGunData == null) { return; }
		GunScriptableObject gunSO = primaryGunData;
		primaryGunData = secondaryGunData;
		secondaryGunData = gunSO;
		inSwap = !inSwap;
		if (reloadCoroutine != null)
		{
				isReloading = false;
				StopCoroutine(reloadCoroutine);
		}
		int num = bulletsInMag;
		bulletsInMag = bulletsInSecondary;
		bulletsInSecondary = num;
		//uIManager.SwitchGun(weaponSwitchSpeed);
		//armsAnim.SetFloat("SwithGunSpeed", 1f / weaponSwitchSpeed);
		//StartCoroutine(UpdateGun(reload: false));
	}

	public void ChangeGun(GunObject gunObject)
	{
		if (secondaryGunData == null)
		{
			secondaryGunData = primaryGunData;
			primaryGunData = gunObject.TakeItem();
			bulletsInSecondary = bulletsInMag;
			//	uIManager.UpdateSprites(primaryGunData, secondaryGunData);
		}
		else
		{
			primaryGunData = gunObject.ReplaceItem(primaryGunData);
			if (inSwap)
			{
			//		uIManager.UpdateSprites(secondaryGunData, primaryGunData);
			}
			else
			{
			//		uIManager.UpdateSprites(primaryGunData, secondaryGunData);
			}
		}
		bulletsInMag = primaryGunData.magazineSize;
			//StartCoroutine(UpdateGun(reload: false));
	}

}
