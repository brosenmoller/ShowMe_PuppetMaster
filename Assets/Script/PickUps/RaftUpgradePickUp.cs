using System.Collections;
using UnityEngine;

public class RaftUpgradePickUp : PickUpInteractable
{
	public GameObject item;

	public GameObject upgrade;

	public bool pickedUp;

	public bool liftUpgrade;

	[SerializeField]
	private float disapearDuration = 0.2f;

	[SerializeField]
	private AnimationCurve animationCurveIn;

	[SerializeField]
	private AnimationCurve animationCurveOut;

	public void PickUp()
	{
		StopAllCoroutines();
		pickedUp = true;
		StartCoroutine(Scale(down: true));
	}

	public void Place()
	{
		item.SetActive(value: false);
		upgrade.SetActive(value: true);
		StopAllCoroutines();
		StartCoroutine(Scale(down: false));
	}

	private IEnumerator Scale(bool down)
	{
		float from = 0f;
		float to = 1f;
		AnimationCurve curve = animationCurveOut;
		if (down)
		{
			from = 1f;
			to = 0f;
			curve = animationCurveIn;
		}
		float t = 0f;
		while (t < disapearDuration)
		{
			base.transform.localScale = Vector3.one * curve.Evaluate(Mathf.Lerp(from, to, t / disapearDuration));
			t += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}
		base.gameObject.SetActive(!down);
	}
}
