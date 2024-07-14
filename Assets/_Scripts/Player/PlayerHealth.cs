using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerController _playerController;

    public int health = 80;
    public int maxHealth = 100;

    public PlayerHealth(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public int Health { get => health; set => health = value; }

    public void TakeDamage(DamageType damageType, int damage)
    {
        _playerController.IsGetingHit = true;

        health = Mathf.Clamp(health - damage, 0, maxHealth);
        Debug.Log("Player HP:" + health);

        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _playerController.IsDead = true;
        Debug.Log("Game Over. Press R to restart");
    }
}
