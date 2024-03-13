public class DamageIce : DamageDecorator
{
    private int _damageIce;
    private DamageType _damageType;

    public DamageIce(IBaseDamage baseDamage, int damageIce, DamageType damageType) : base(baseDamage)
    {
        _damageIce = damageIce;
        _damageType = damageType;
    }

    public override void ApplyDamage(IDamageable damage)
    {
        base.ApplyDamage(damage);

        damage.TakeDamage(_damageType, _damageIce);
    }
}

