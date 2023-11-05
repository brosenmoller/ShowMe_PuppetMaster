using PrimeTween;
using UnityEngine;

public class UpgradePickupAnimation : MonoBehaviour
{
    private void Awake()
    {
        Tween.LocalPositionY(transform, 0.0f, 2, Ease.InOutSine, -1, CycleMode.Yoyo);
        Tween.EulerAngles(transform.GetChild(0), startValue: Vector3.zero, endValue: new Vector3(0, 360), duration: 9, Ease.Linear, -1, CycleMode.Restart);
    }
}
