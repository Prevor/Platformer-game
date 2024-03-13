using System;
using UnityEngine;

public class HealthPotion : MonoBehaviour, ICollectible
{
    public static event Action OnHealthPotionUsed;

    [SerializeField] private ParticleSystem healathEffect;

    public void Collect()
    {
        OnHealthPotionUsed?.Invoke();

        if (!UpdateHealthBar.isMaxHealthBar)
        {
            healathEffect.Play();
            Destroy(gameObject);
        }
    }
}
