using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteractable : MonoBehaviour
{
	public enum itemType
	{
		gun = 0,
		modifier = 1,
		raftPart = 2
	}

	public itemType typeItem;
}

