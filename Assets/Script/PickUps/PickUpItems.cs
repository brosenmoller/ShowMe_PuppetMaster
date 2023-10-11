using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItems : MonoBehaviour
{
	[SerializeField]
	private LayerMask pickupMask;

	[SerializeField]
	private LayerMask raftMask;

	[SerializeField]
	private Gun gun;

	[SerializeField]
	private float interactionRange = 5f;

	[SerializeField]
	private float moveForce = 250f;

	[SerializeField]
	private Transform holdParent;

	[SerializeField]
	private float smallScale;

	[SerializeField]
	private GameObject heldObject;

	private PlayerController player;

	//private Inventory inventory;

	[SerializeField]
	private bool isHolding;
	public float rayLength = 3.5f;
	public LayerMask Attach;

	private void Awake()
	{
		player = GetComponent<PlayerController>();
		//inventory = GetComponent<Inventory>();
	}

	public void OnInteract()
	{
		PickPlaceBoatParts();
		PickupItems();
	}
	public void OnStopInteract()
    {
		isHolding = false;
	}
	private void PickupItems()
	{
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (!Physics.Raycast(ray.origin, ray.direction, out var hitInfo, interactionRange, pickupMask))
		{
			return;
		}
		switch (hitInfo.transform.GetComponent<PickUpInteractable>().typeItem)
		{
			case PickUpInteractable.itemType.gun:
				{
					GunObject component2 = hitInfo.transform.GetComponent<GunObject>();
					gun.ChangeGun(component2);
					break;
				}
			/*case PickUpInteractable.itemType.modifier:
				{
					ModifierPickup component = hitInfo.transform.GetComponent<ModifierPickup>();
					if (component is BulletModifierPickup)
					{
							inventory.AddBulletModifier(component as BulletModifierPickup);
					}
					else if (component is GunModifierPickup)
					{
							inventory.AddGunMod(component as GunModifierPickup);
					}
					else if (component is PlayerModifierPickup)
					{
							inventory.AddPlayerModifier(component as PlayerModifierPickup);
					}
					break;
				}*/
		}
	}

	private void PickPlaceBoatParts()
	{
		
			isHolding = true;
			if (player.raftUpgrade == null)
			{
				if (Physics.Raycast(player.playerCam.transform.position, player.playerCam.transform.forward, out var hitInfo, rayLength, raftMask, QueryTriggerInteraction.Ignore))
				{
					player.raftUpgrade = hitInfo.collider.transform.root.GetComponent<RaftUpgradePickUp>();
					player.isHolding = false;
					//inventory.HoldItemAnim(isHolding: true);
					player.raftUpgrade.PickUp();
				}
			}
			else
			{
				if (!Physics.Raycast(player.playerCam.transform.position, player.playerCam.transform.forward, out var hitInfo2, rayLength, Attach, QueryTriggerInteraction.Collide))
				{
					return;
				}
				UpgradePoint component = hitInfo2.transform.GetComponent<UpgradePoint>();
				if (component.CanUpgrade(player.raftUpgrade))
				{
					player.raftUpgrade = null;
					player.isHolding = false;
					//inventory.HoldItemAnim(isHolding: false);
				}
			}
		
			/*if (!context.performed || !isHolding || !(player.raftUpgrade == null) || !Physics.Raycast(player.playerCam.transform.position, player.playerCam.transform.forward, out var hitInfo3, rayLength, Attach, QueryTriggerInteraction.Collide) || !hitInfo3.transform.GetComponent<UpgradePoint>())
			{
				return;
			}
			UpgradePoint component2 = hitInfo3.transform.GetComponent<UpgradePoint>();
			if ((bool)component2.GetUpgrade())
			{
				//player.HoldItemAnim(isHolding: true);
				player.raftUpgrade = component2.TakeUpgrade();
				player.raftUpgrade.PickUp();
			}*/
	}
}
