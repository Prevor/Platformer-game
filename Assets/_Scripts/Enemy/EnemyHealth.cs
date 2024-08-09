using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable, IBurnable
{
    [SerializeField] private TextMeshPro _healthText;

    [SerializeField] private int _health;
    public int Health { get => _health; set => _health = value; }

    [SerializeField] private bool _isBurning;
    public bool IsBurning { get => _isBurning; set => _isBurning = value; }

    private Coroutine _burningCoroutine;

    public event DeathEvent OnDeath;
    public delegate void DeathEvent(Enemy enemy);

    public void TakeDamage(DamageType damageType, int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            Die();
            OnDeath?.Invoke(GetComponent<Enemy>());
            StopBurning();
        }

        Debug.Log($"Damage received. Type {damageType}, damage: {damage}");
    }

    public void StartBurning(int damagePerSecond)
    {
        _isBurning = true;
        if (_burningCoroutine != null)
        {
            StopCoroutine(_burningCoroutine);
        }

        _burningCoroutine = StartCoroutine(Burn(damagePerSecond));
    }

    private IEnumerator Burn(int damagePerSecond)
    {
        float minTimeToDamage = 1f / damagePerSecond;
        WaitForSeconds wait = new(damagePerSecond);
        int damagePerTick = Mathf.CeilToInt(minTimeToDamage) + 1;

        TakeDamage(DamageType.Fire, damagePerTick);

        while (_isBurning)
        {
            yield return wait;
            TakeDamage(DamageType.Fire, damagePerTick);
        }
    }

    public void StopBurning()
    {
        _isBurning = false;
        if (_burningCoroutine != null)
        {
            StopCoroutine(_burningCoroutine);
        }
    }

    private void Die()
    {
        Debug.Log("Enemy die");
    }

    public void KnocBack(Vector3 direction, float force)
    {
        throw new System.NotImplementedException();
    }
}

