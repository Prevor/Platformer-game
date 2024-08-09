using UnityEngine;

public interface IDamageable
{
    public int Health { get; set; }
    void TakeDamage(DamageType damageType, int damage);
    void KnocBack(Vector3 direction, float force);
}
    