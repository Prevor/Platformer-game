using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private TextMeshPro _healthText;

    [SerializeField] private int _health;
    public int Health { get => _health; set => _health = value; }

    public void TakeDamage(DamageType damageType, int damage)
    {
        _health -= damage;
        _healthText.SetText(_health.ToString());
        if (_health <= 0)
        {
            _health = 0;

            Die();
        }

        Debug.Log($"Damage received. Type {damageType}, damage: {damage} | HP {_health}");
    }

    private void Die()
    {
        _healthText.SetText("I'm dead");
    }
}
