using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStream : MonoBehaviour
{
	public float speed;

	[HideInInspector]
	public Vector3 calculatedDirection;

	public Vector3 direction;

	public bool exiting;

	public bool drawGizmo = true;

	private PhysicsRaft raft;

	private void Start()
	{
		direction = base.transform.forward * speed;
		raft = Object.FindObjectOfType<PhysicsRaft>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Raft"))
		{
			calculatedDirection = direction;
			exiting = false;
			raft.activeWaterStreams.Add(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ( other.CompareTag("Raft"))
		{
			raft.activeWaterStreams.Remove(this);
		}
	}

	private void OnValidate()
	{
		direction = base.transform.forward * speed;
	}


	private void OnDrawGizmos()
	{
		if (drawGizmo)
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(base.transform.position, base.transform.lossyScale);
			Gizmos.matrix = matrix;
			DrawArrow.ForGizmo(base.transform.position - base.transform.forward * base.transform.localScale.z / 2f, base.transform.forward * base.transform.localScale.z, 0.5f);
			int num = Mathf.RoundToInt(base.transform.localScale.x / 3f);
			for (int i = 1; i < num + 1; i++)
			{
				Vector3 pos = base.transform.position - base.transform.forward * base.transform.localScale.z / 2f + base.transform.right * i;
				Vector3 pos2 = base.transform.position - base.transform.forward * base.transform.localScale.z / 2f + -base.transform.right * i;
				DrawArrow.ForGizmo(pos, base.transform.forward * base.transform.localScale.z, 0.5f);
				DrawArrow.ForGizmo(pos2, base.transform.forward * base.transform.localScale.z, 0.5f);
			}
		}
	}
}
