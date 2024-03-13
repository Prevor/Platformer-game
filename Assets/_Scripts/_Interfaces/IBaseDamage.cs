public interface IBaseDamage
{
    int GetDamage();
    DamageType GetDamageType();
    void ApplyDamage(IDamageable damage);
}   