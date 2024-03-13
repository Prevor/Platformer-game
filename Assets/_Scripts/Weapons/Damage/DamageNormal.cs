public class DamageNormal : IBaseDamage
{
    private int _damage;
    private DamageType _damageType;

    public DamageNormal(int damage, DamageType damageType)
    {
        _damage = damage;
        _damageType = damageType;
    }

    public void ApplyDamage(IDamageable damage)
    {
        damage.TakeDamage(_damageType, _damage);
    }

    public int GetDamage()
    {
        return _damage;
    }

    public DamageType GetDamageType()
    {
        return _damageType;
    }
}

