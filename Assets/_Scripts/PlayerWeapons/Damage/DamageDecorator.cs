public abstract class DamageDecorator : IBaseDamage
{
    protected IBaseDamage mainDamage;

    protected DamageDecorator(IBaseDamage baseDamage)
    {
        mainDamage = baseDamage;
    }

    public virtual int GetDamage()
    {
        return mainDamage.GetDamage();
    }

    public virtual DamageType GetDamageType()
    {
        return mainDamage.GetDamageType();
    }

    public virtual void ApplyDamage(IDamageable i_damage)
    {
        mainDamage.ApplyDamage(i_damage);
    }

}

