using TMPro;
using UnityEngine;

public class ShieldGenerator : InteractableObject
{
    [Header("Shield Generator Settings")]
    [SerializeField] private int activeDuration;
    [SerializeField] private int resetDelay;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject shield;
 
    private bool canBeDeployed = true;
    private bool isDeployed = false;

    private Timer activeTimer;
    private Timer delayTimer;

    private void Start()
    {
        activeTimer = new Timer(activeDuration, CancelShield, false);
        delayTimer = new Timer(resetDelay, ResetShield, false);
    }

    private void Update()
    {
        if (isDeployed)
        {
            timerText.text = $"{(int)activeTimer.TimeLeft}";
            IsInteractable = false;
        }
        else if (!canBeDeployed)
        {
            timerText.text = $"{(int)delayTimer.TimeLeft}";
        }
        else
        {
            timerText.text = "V";
            IsInteractable = true;
        }
    }

    public override bool CheckIfLocked()
    {
        return isDeployed || canBeDeployed;
    }

    protected override void PerformInteraction()
    {
        if (!canBeDeployed || isDeployed) { return; }

        shield.SetActive(true);
        isDeployed = true;
        canBeDeployed = false;
        activeTimer.Reset();
        activeTimer.Start();
    }

    private void CancelShield()
    {
        isDeployed = false;
        shield.SetActive(false);
        delayTimer.Reset();
        delayTimer.Start();
    }

    private void ResetShield()
    {
        canBeDeployed = true;
    }
}
