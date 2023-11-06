using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageAble
{
    public int maxHealth = 4;
    [HideInInspector] public int health;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private VolumeProfile postProcessingVolume;
    private Vignette vignetteEffect;

    private PlayerHealthUI healthUI;

    private void Awake()
    {
        healthUI = FindObjectOfType<PlayerHealthUI>();
        health = maxHealth;

        if (postProcessingVolume.TryGet(out Vignette vignette))
        {
            vignetteEffect = vignette;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthUI.LooseHeart(damage);

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            vignetteEffect.intensity.overrideState = true;
            Tween.Custom(.1f, .5f, duration: .1f, onValueChange: newValue => vignetteEffect.intensity.value = newValue, ease: Ease.InSine)
                .OnComplete(() => Tween.Custom(.5f, .1f, duration: .2f, onValueChange: newValue => vignetteEffect.intensity.value = newValue, ease: Ease.OutSine));
        }
    }

    public void Revive(int amount)
    {
        health += amount;
        if (health > maxHealth) { health = maxHealth; }

        healthUI.ReviveHeart(amount);
    }

    public void AddAdditionalHearts(int amount)
    {
        healthUI.AddAddionalHearts(amount);
    }

    public void DamageAndRespawn()
    {
        TakeDamage(1);
        Respawn();
    }

    public void Respawn()
    {
        transform.position = respawnPoint.position;
    }
}
